using Features.Animator.Components;
using Features.Character.Components;
using Features.StateMachine.Components.Core;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Systems.Core;
using Features.StateMachine.Systems.Core.SystemGroups;
using Features.StateMachine.Systems.Nodes.Leaf;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[assembly: RegisterGenericJobType(typeof(PlayAnimationSystem.ExecuteNodesJob<PlayAnimation, PlayAnimationSystem.Processor>))]
namespace Features.StateMachine.Systems.Nodes.Leaf
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