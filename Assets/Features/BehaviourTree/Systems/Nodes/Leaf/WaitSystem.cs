using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(WaitSystem.ExecuteNodesJob<Wait, WaitSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
	[UpdateInGroup(typeof(LeafNodeSystemGroup))]
    public partial class WaitSystem : NodeExecutorSystem<Wait, WaitSystem.Processor>
    {
        protected override WaitSystem.Processor PrepareProcessor()
        {
            return new WaitSystem.Processor
            {
                DeltaTime = UnityEngine.Time.deltaTime
            };
        }
        
        [BurstCompile]
        public struct Processor : INodeProcessor<Wait>
        {
            public float DeltaTime;
            
            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref Wait nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                nodeComponent.CurrentWaitTime = nodeComponent.WaitTime;

                return NodeResult.Running;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref Wait nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                nodeComponent.CurrentWaitTime -= DeltaTime;

                return nodeComponent.CurrentWaitTime < 0 
                    ? NodeResult.Success 
                    : NodeResult.Running;
            }
        }
    }
}