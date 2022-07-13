using Features.Animator.Components;
using Features.Character.Components;
using Unity.Entities;

namespace Features.Animator.Systems
{
    public partial class PlayNextAnimationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<AnimatorWrapper, PlayNextAnimation>()
                .ForEach((ref PlayNextAnimation nextAnimation, in AnimatorWrapper animator) =>
                {
                    if (nextAnimation.Enable)
                    {
                        var animationName = nextAnimation.AnimationName.ToString();
                        animator.Value.Play(animationName);

                        nextAnimation.Enable = false;
                    }
                })
                .WithoutBurst()
                .Run();
        }
    }
}