using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Features.Character.Components;
using Features.StateMachine.Systems.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(MoveToTargetSystem.ExecuteNodesJob<MoveToTarget, MoveToTargetSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
    public class MoveToTargetSystem : NodeExecutorSystem<MoveToTarget, MoveToTargetSystem.Processor>
    {
        protected override MoveToTargetSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
            return new MoveToTargetSystem.Processor
            {
                SelfTargetComponents = GetComponentDataFromEntity<Target>(),
                SelfTranslationComponents = GetComponentDataFromEntity<Translation>(),
                
                SelfMovementComponents = GetComponentDataFromEntity<Movement>(),
                SelfSpeedComponents = GetComponentDataFromEntity<Speed>(),
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<MoveToTarget>
        {
            [ReadOnly]
            public ComponentDataFromEntity<Target> SelfTargetComponents; 
            [ReadOnly] 
            public ComponentDataFromEntity<Translation> SelfTranslationComponents; 
            
            public ComponentDataFromEntity<Movement> SelfMovementComponents;
            public ComponentDataFromEntity<Speed> SelfSpeedComponents;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref MoveToTarget nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                return NodeResult.Running;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref MoveToTarget nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var target = SelfTargetComponents[rootEntity];
                var translation = SelfTranslationComponents[rootEntity];
                
                var movement = SelfMovementComponents[rootEntity];
                var speed = SelfSpeedComponents[rootEntity];

                NodeResult result;

                var distance = math.distance(target.Value, translation.Value);
                
                if (distance < nodeComponent.AcceptableRadius)
                {
                    movement.Direction = float3.zero;
                    movement.Enable = false;

                    result = NodeResult.Success;
                }
                else
                {
                    var direction = math.normalize(target.Value - translation.Value);

                    movement.Direction = direction;
                    movement.Enable = math.any(movement.Direction != float3.zero);
                
                    speed.Value = nodeComponent.Speed;

                    result = NodeResult.Running;
                }

                SelfMovementComponents[rootEntity] = movement;
                SelfSpeedComponents[rootEntity] = speed;

                return result;
            }
        }
    }
}