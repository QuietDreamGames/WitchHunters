using Features.Character.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(GetClosestPlayerSystem.ExecuteNodesJob<GetClosestPlayer, GetClosestPlayerSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Leaf
{
    public partial class GetClosestPlayerSystem : NodeExecutorSystem<GetClosestPlayer, GetClosestPlayerSystem.Processor>
    {
        protected override GetClosestPlayerSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
			var targetPlayersQuery = GetEntityQuery(typeof(PlayerTag), ComponentType.ReadOnly<Translation>());
            
            return new GetClosestPlayerSystem.Processor
            {
                TargetsTranslation = targetPlayersQuery.ToComponentDataArray<Translation>(Allocator.TempJob),

                SelfTargetComponents = GetComponentDataFromEntity<Target>(),
                SelfTranslationComponents = GetComponentDataFromEntity<Translation>(),
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<GetClosestPlayer>
        {
            [ReadOnly] 
            [DeallocateOnJobCompletion] 
            public NativeArray<Translation> TargetsTranslation;
            
            [ReadOnly]
            public ComponentDataFromEntity<Translation> SelfTranslationComponents;
            
            public ComponentDataFromEntity<Target> SelfTargetComponents;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref GetClosestPlayer nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                if (TargetsTranslation.Length > 0)
                {
                    var targetComponent = SelfTargetComponents[rootEntity];
                    var translationComponent = SelfTranslationComponents[rootEntity];

                    var (currentTarget, currentDistance) = GetClosestTarget(in translationComponent);

                    if (currentDistance < nodeComponent.Distance)
                    {
                        targetComponent.Value = currentTarget.Value;
                        SelfTargetComponents[rootEntity] = targetComponent;
                        
                        return NodeResult.Success;
                    }

                    return NodeResult.Failed;
                }

                return NodeResult.Failed;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref GetClosestPlayer nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                return NodeResult.Running;
            }

            private (Translation, float) GetClosestTarget(in Translation translation)
            {
                var currentTarget = TargetsTranslation[0];
                var currentDistance = math.distance(translation.Value, currentTarget.Value);

                for (int i = 1; i < TargetsTranslation.Length; i++)
                {
                    var newTarget = TargetsTranslation[i];
                    var newDistance = math.distance(translation.Value, newTarget.Value);

                    if (newDistance < currentDistance)
                    {
                        currentTarget = newTarget;
                        currentDistance = newDistance;
                    }
                }
                
                return (currentTarget, currentDistance);
            }
        }
    }
}