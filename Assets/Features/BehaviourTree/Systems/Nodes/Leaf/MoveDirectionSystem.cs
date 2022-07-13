using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Features.Character.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[assembly: RegisterGenericJobType(typeof(MoveDirectionSystem.ExecuteNodesJob<MoveDirection, MoveDirectionSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
	[UpdateInGroup(typeof(LeafNodeSystemGroup))]
    public partial class MoveDirectionSystem : NodeExecutorSystem<MoveDirection, MoveDirectionSystem.Processor>
    {
        protected override MoveDirectionSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
            return new MoveDirectionSystem.Processor
            {
                AllMovements = GetComponentDataFromEntity<Movement>(), 
                DeltaTime = UnityEngine.Time.deltaTime
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<MoveDirection>
        {
            public ComponentDataFromEntity<Movement> AllMovements;

            public float DeltaTime;
            
            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref MoveDirection nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var duration = nodeComponent.Time;
                nodeComponent.CurrentTime = duration;

                return NodeResult.Running;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref MoveDirection nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                nodeComponent.CurrentTime -= DeltaTime;

                if (nodeComponent.CurrentTime < 0)
                { 
                    ChangeMovementDirection(in rootEntity, float3.zero);
                    return NodeResult.Success;
                }

                ChangeMovementDirection(in rootEntity, nodeComponent.Direction);
                return NodeResult.Running;
            }

            private void ChangeMovementDirection(in Entity rootEntity, float3 direction)
            {
                var movement = AllMovements[rootEntity];
                
                movement.Direction = direction;
                movement.Enable = math.any(movement.Direction != float3.zero);
                
                AllMovements[rootEntity] = movement;
            }
        }
    }
}