using Features.Character.Components;
using Features.StateMachine.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Systems;
using Features.StateMachine.Systems.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[assembly: RegisterGenericJobType(typeof(MoveDirectionSystem.ExecuteNodesJob<MoveDirection, MoveDirectionSystem.Processor>))]
namespace Features.StateMachine.Systems
{
    public partial class MoveDirectionSystem : NodeExecutorSystem<MoveDirection, MoveDirectionSystem.Processor>
    {
        protected override Processor PrepareProcessor()
        {
            return new Processor
            {
                AllMovements = GetComponentDataFromEntity<Movement>(), DeltaTime = UnityEngine.Time.deltaTime
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<MoveDirection>
        {
            [ReadOnly] public ComponentDataFromEntity<Movement> AllMovements;

            public float DeltaTime;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity agentEntity, ref MoveDirection nodeComponent,
                int indexOfFirstEntityInQuery, int iterIndex)
            {
                var duration = nodeComponent.Time;
                nodeComponent.CurrentTime = duration;

                return NodeResult.Running;
            }

            public NodeResult Update(in Entity agentEntity, ref MoveDirection nodeComponent,
                int indexOfFirstEntityInQuery, int iterIndex)
            {
                nodeComponent.CurrentTime -= DeltaTime;

                if (nodeComponent.CurrentTime < 0)
                {
                    return NodeResult.Success;
                }

                var movement = AllMovements[agentEntity];
                movement.Direction = nodeComponent.Direction;
                movement.Enable = math.any(movement.Direction != float3.zero);
                AllMovements[agentEntity] = movement;

                return NodeResult.Running;
            }
        }
    }
}