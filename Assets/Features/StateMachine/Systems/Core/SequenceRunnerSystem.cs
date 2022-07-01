using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Features.StateMachine.Systems.Core
{
    [UpdateBefore(typeof(SequenceIteratorSystem))]
    public partial class SequenceRunnerSystem : SystemBase
    {
        private EntityQuery _nodesQuery;

        protected override void OnCreate()
        {
            _nodesQuery = GetEntityQuery(typeof(NodeComponent));
        }

        protected override void OnUpdate()
        {
            var job = new Job
            {
                NodeComponentType = GetComponentTypeHandle<NodeComponent>(),
                AllAgents = GetComponentDataFromEntity<NodeAgent>()
            };

            job.ScheduleParallel(_nodesQuery);
        }

        [BurstCompile]
        private new struct Job : IJobEntityBatch
        {
            public ComponentTypeHandle<NodeComponent> NodeComponentType;

            [ReadOnly] public ComponentDataFromEntity<NodeAgent> AllAgents;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
            {
                var nodes = batchInChunk.GetNativeArray(NodeComponentType);

                for (int i = 0; i < nodes.Length; i++)
                {
                    var node = nodes[i];
                    var agent = AllAgents[node.AgentEntity];

                    node.IsExec = agent.CurrentNodeIndex == node.ActionIndex;
                    node.Started = false;
                    nodes[i] = node;
                }
            }
        }
    }
}