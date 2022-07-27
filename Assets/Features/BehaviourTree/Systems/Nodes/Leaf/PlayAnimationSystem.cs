using Features.Animator.Components;
using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Systems.Core;
using Features.BehaviourTree.Systems.Core.SystemGroups;
using Features.BehaviourTree.Systems.Nodes.Leaf;
using Features.Character.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(PlayAnimationSystem.ExecuteNodesJob<PlayAnimation, PlayAnimationSystem.Processor>))]
namespace Features.BehaviourTree.Systems.Nodes.Leaf
{
	[UpdateInGroup(typeof(LeafNodeSystemGroup))]
    public partial class PlayAnimationSystem : NodeExecutorSystem<PlayAnimation, PlayAnimationSystem.Processor>
    {
        protected override PlayAnimationSystem.Processor PrepareProcessor()
        {
            ShouldScheduleParallel = false;
            
			var targetPlayersQuery = GetEntityQuery(typeof(PlayerTag), ComponentType.ReadOnly<Translation>());
            
            return new PlayAnimationSystem.Processor
            {
                SelfPlayNextAnimationComponents = GetComponentDataFromEntity<PlayNextAnimation>(),
            };
        }

        [BurstCompile]
        public struct Processor : INodeProcessor<PlayAnimation>
        {
            public ComponentDataFromEntity<PlayNextAnimation> SelfPlayNextAnimationComponents;

            public void BeforeChunkIteration(ArchetypeChunk batchInChunk, int batchIndex)
            {
                
            }

            public NodeResult Start(in Entity rootEntity,
                in Entity nodeEntity,
                ref PlayAnimation nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                var playNextAnimationComponent = SelfPlayNextAnimationComponents[rootEntity];

                playNextAnimationComponent.AnimationName = nodeComponent.AnimationName;
                playNextAnimationComponent.Enable = true;

                SelfPlayNextAnimationComponents[rootEntity] = playNextAnimationComponent;

                return NodeResult.Success;
            }

            public NodeResult Update(in Entity rootEntity,
                in Entity nodeEntity,
                ref PlayAnimation nodeComponent,
                int indexOfFirstEntityInQuery,
                int iterIndex)
            {
                return NodeResult.Success;
            }
        }
    }
}