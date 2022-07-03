﻿using Features.StateMachine.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Systems;
using Features.StateMachine.Systems.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Features.StateMachine.Systems.Core
{
    public abstract partial class NodeExecutorSystem<TNodeFilter, TProcessor> : SystemBase
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

            var jobHandle = ShouldScheduleParallel
                ? job.ScheduleParallel(_query, Dependency)
                : job.Schedule(_query, Dependency);
            Dependency = jobHandle;
        }

        [BurstCompile]
        public struct ExecuteNodesJob<TNodeFilterJob, TProcessorJob> : IJobEntityBatchWithIndex
            where TNodeFilterJob : struct, INodeComponent 
            where TProcessorJob : struct, INodeProcessor<TNodeFilterJob>
        {
            [NativeDisableParallelForRestriction] 
            public ComponentTypeHandle<NodeComponent> NodeComponentType;
            public ComponentTypeHandle<TNodeFilterJob> NodeFilterType;

            public bool IsActionFilterHasArray;
            public TProcessorJob Processor;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex, int indexOfFirstEntityInQuery)
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
                        ExecuteNode(ref node, ref nodeFilter, indexOfFirstEntityInQuery, i);
                        nodeFilters[i] = nodeFilter;
                    }
                    else
                    {
                        ExecuteNode(ref node, ref defaultActionFilter, indexOfFirstEntityInQuery, i);
                    }

                    if (node.Result is NodeResult.Success or NodeResult.Failed)
                    {
                        node.IsExec = false;
                    }

                    nodes[i] = node;
                }
            }

            private void ExecuteNode(ref NodeComponent nodeComponent, ref TNodeFilterJob actionFilter,
                int indexOfFirstEntityInQuery, int iterIndex)
            {
                if (!nodeComponent.Started)
                {
                    nodeComponent.Result = Processor.Start(nodeComponent.RootEntity,
                        nodeComponent.AgentEntity,
                        ref actionFilter,
                        indexOfFirstEntityInQuery,
                        iterIndex);
                    nodeComponent.Started = true;

                    if (nodeComponent.Result is NodeResult.Success or NodeResult.Failed)
                    {
                        return;
                    }
                }

                nodeComponent.Result = Processor.Update(nodeComponent.RootEntity,
                    nodeComponent.AgentEntity,
                    ref actionFilter,
                    indexOfFirstEntityInQuery,
                    iterIndex);
            }
        }

        protected abstract TProcessor PrepareProcessor();

        protected virtual void AfterJobScheduling(in JobHandle handle)
        {
            // Routines like calling AddJobHandleForProducer() may be placed here
        }

        protected virtual bool ShouldScheduleParallel { get; set; } = true;

        protected ref readonly EntityQuery Query => ref _query;
    }
}