using Features.StateMachine.Components.Core;
using Features.StateMachine.Systems.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Features.StateMachine.Systems.Core
{
    public abstract partial class SequenceExecutorSystem<TNodeFilter, TProcessor> : SystemBase
        where TNodeFilter : struct, INodeComponent 
        where TProcessor : struct, INodeProcessor<TNodeFilter>
    {
        private EntityQuery _query;

        protected bool IsActionFilterHasArray;

        protected override void OnCreate()
        {
            _query = PrepareQuery();

            IsActionFilterHasArray = !TypeManager.GetTypeInfo(TypeManager.GetTypeIndex<TNodeFilter>()).IsZeroSized;
        }

        protected virtual EntityQuery PrepareQuery()
        {
            return GetEntityQuery(typeof(NodeComponent), typeof(TNodeFilter));
        }

        protected override void OnUpdate()
        {
            var job = new ExecuteNodesJob<TNodeFilter, TProcessor>
            {
                NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
                NodeFilterType = GetComponentTypeHandle<TNodeFilter>(),
                IsActionFilterHasArray = IsActionFilterHasArray,
                Processor = PrepareProcessor()
            };
            
            job.Schedule(_query);

        }

        public struct ExecuteNodesJob<TNodeFilterJob, TProcessorJob> : IJobEntityBatch
            where TNodeFilterJob : struct, INodeComponent 
            where TProcessorJob : struct, INodeProcessor<TNodeFilterJob>
        {
            public ComponentTypeHandle<NodeComponent> NodeComponentType;
            public ComponentTypeHandle<TNodeFilterJob> NodeFilterType;

            public bool IsActionFilterHasArray;
            public TProcessorJob Processor;

            [NativeDisableParallelForRestriction] 
            public ComponentDataFromEntity<NodeAgent> AllAgents;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
            {
                var nodes = batchInChunk.GetNativeArray(NodeComponentType);
                var nodeFilters = IsActionFilterHasArray 
                    ? batchInChunk.GetNativeArray(NodeFilterType) 
                    : default;
                TNodeFilterJob defaultActionFilter = default;

                Processor.BeforeChunkIteration(batchInChunk, batchIndex);

                for (int i = 0; i < batchInChunk.Count; i++)
                {
                    var node = nodes[i];
                    if (!node.IsExec)
                    {
                        continue;
                    }

                    if (IsActionFilterHasArray)
                    {
                        var nodeFilter = nodeFilters[i];
                        ExecuteNode(ref node, ref nodeFilter, 0, i);
                        nodeFilters[i] = nodeFilter;
                    }
                    else
                    {
                        ExecuteNode(ref node, ref defaultActionFilter, 0, i);
                    }

                    if (node.Result == NodeResult.Failed)
                    {
                        node.Started = false;
                    }

                    nodes[i] = node;

                    var agent = AllAgents[node.AgentEntity];
                    agent.LastResult = node.Result;
                    AllAgents[node.AgentEntity] = agent;
                }
            }

            private void ExecuteNode(ref NodeComponent nodeComponent, ref TNodeFilterJob actionFilter,
                int indexOfFirstEntityInQuery, int iterIndex)
            {
                if (!nodeComponent.Started)
                {
                    nodeComponent.Result = Processor.Start(nodeComponent.AgentEntity, ref actionFilter,
                        indexOfFirstEntityInQuery, iterIndex);
                    nodeComponent.Started = true;

                    if (nodeComponent.Result == NodeResult.Success || nodeComponent.Result == NodeResult.Failed)
                    {
                        return;
                    }
                }

                nodeComponent.Result = Processor.Update(nodeComponent.AgentEntity, ref actionFilter,
                    indexOfFirstEntityInQuery, iterIndex);
            }
        }

        protected abstract TProcessor PrepareProcessor();

        protected virtual void AfterJobScheduling(in JobHandle handle)
        {
            // Routines like calling AddJobHandleForProducer() may be placed here
        }

        protected virtual bool ShouldScheduleParallel => true;

        protected ref readonly EntityQuery Query => ref _query;
    }
}