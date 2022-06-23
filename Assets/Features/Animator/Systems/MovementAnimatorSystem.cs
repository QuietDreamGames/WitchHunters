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
                    animator.Value.SetFloat(conf.Horizontal, movement.Direction.x); 
                    animator.Value.SetFloat(conf.Vertical, movement.Direction.y);
                    animator.Value.SetBool(conf.Moving, movement.Enable);
                })
                .WithoutBurst()
                .Run();
        }
    }
}