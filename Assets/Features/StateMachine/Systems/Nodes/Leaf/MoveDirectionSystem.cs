using Features.Character.Components;
using Features.StateMachine.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[assembly: RegisterGenericJobType(typeof(MoveDirectionSystem.ExecuteNodesJob<MoveDirection, MoveDirectionSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Leaf
{
    public partial class MoveDirectionSystem : NodeExecutorSystem<MoveDirection, MoveDirectionSystem.Processor>
    {
        protected override Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
            return new Processor
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