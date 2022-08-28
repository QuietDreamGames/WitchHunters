using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Features.Character.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(FaceToTargetSystem.ExecuteNodesJob<FaceToTarget, FaceToTargetSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
	[UpdateInGroup(typeof(LeafNodeSystemGroup))]
    public class FaceToTargetSystem : NodeExecutorSystem<FaceToTarget, FaceToTargetSystem.Processor>
    {
        protected override FaceToTargetSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
            return new FaceToTargetSystem.Processor
            {
                SelfTargetComponents = GetComponentDataFromEntity<Target>(),
                SelfTranslationComponents = GetComponentDataFromEntity<Translation>(),
                
                SelfMovementComponents = GetComponentDataFromEntity<Movement>(),
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<FaceToTarget>
        {
            [ReadOnly] public ComponentDataFromEntity<Target> SelfTargetComponents;
            [ReadOnly] public ComponentDataFromEntity<Translation> SelfTranslationComponents;

            public ComponentDataFromEntity<Movement> SelfMovementComponents;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {

            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref FaceToTarget nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var targetComponent = SelfTargetComponents[rootEntity];
                var translationComponent = SelfTranslationComponents[rootEntity];
                
                var movementComponent = SelfMovementComponents[rootEntity];

                var direction = math.normalize(targetComponent.Value - translationComponent.Value);
                movementComponent.Direction = direction;
                movementComponent.Trigger = true;

                SelfMovementComponents[rootEntity] = movementComponent;
                
                return NodeResult.Success;
            }
            
            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref FaceToTarget nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                return NodeResult.Running;
            }
        }
    }
}