using Features.Animator.Components;
using Features.Character.Components;
using Features.Character.Systems;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Animator.Systems
{
    [UpdateAfter(typeof(MovementSystem))]
    public partial class MovementAnimatorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Movement, AnimatorWrapper, AnimatorConfiguration>()
                .ForEach((in AnimatorWrapper animator, in Movement movement, in AnimatorConfiguration conf) =>
                {
                    var moving = math.any(movement.Value != float2.zero);
                    animator.Value.SetBool(conf.Moving, moving);

                    if (moving)
                    {
                        animator.Value.SetFloat(conf.Horizontal, movement.Value.x);
                        animator.Value.SetFloat(conf.Vertical, movement.Value.y);
                    }
                })
                .WithoutBurst()
                .Run();
        }
    }
}