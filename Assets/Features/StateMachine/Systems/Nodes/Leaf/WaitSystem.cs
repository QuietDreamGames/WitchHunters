using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Nodes.Leaf;
using Unity.Entities;
using Unity.Jobs;

[assembly: RegisterGenericJobType(typeof(WaitSystem.ExecuteNodesJob<Wait, WaitSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Leaf
{
    public partial class WaitSystem : NodeExecutorSystem<Wait, WaitSystem.Processor>
    {
        protected override WaitSystem.Processor PrepareProcessor()
        {
            return new WaitSystem.Processor
            {
                DeltaTime = UnityEngine.Time.deltaTime
            };
        }

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