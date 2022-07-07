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
        protected override void OnCreate()
        {
            Entities
                .WithAll<Movement, AnimatorWrapper, AnimatorConfiguration>()
                .ForEach((in AnimatorWrapper animator, in Movement movement, in AnimatorConfiguration conf) =>
                {
                    animator.Value.SetFloat(conf.Horizontal.ToString(), movement.Direction.x); 
                    animator.Value.SetFloat(conf.Vertical.ToString(), movement.Direction.y);
                    animator.Value.SetBool(conf.Moving.ToString(), movement.Enable);
                })
                .WithoutBurst()
                .Run();
        }
        
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Movement, AnimatorWrapper, AnimatorConfiguration>()
                .ForEach((in AnimatorWrapper animator, in Movement movement, in AnimatorConfiguration conf) =>
                {
                    if (movement.Enable)
                    {
                        animator.Value.SetFloat(conf.Horizontal.ToString(), movement.Direction.x);
                        animator.Value.SetFloat(conf.Vertical.ToString(), movement.Direction.y);
                    }
                    
                    animator.Value.SetBool(conf.Moving.ToString(), movement.Enable);
                })
                .WithoutBurst()
                .Run();
        }
    }
}