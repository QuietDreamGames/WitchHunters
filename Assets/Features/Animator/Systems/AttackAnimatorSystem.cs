using Features.Animator.Components;
using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Systems;
using Unity.Entities;

namespace Features.Animator.Systems
{
    [UpdateAfter(typeof(CharacterInputSystem))]
    [UpdateBefore(typeof(MovementSystem))]
    public partial class AttackAnimatorSystem : SystemBase
    {
        private EntityQuery _query;
        
        protected override void OnStartRunning()
        {
            var desc1 = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Attack) }
            };

            var desc2 = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Movement) }
            };
            
            var desc3 = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(AnimatorWrapper) }
            };

            var desc4 = new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(AnimatorConfiguration) }
            };
            
            _query = GetEntityQuery(new EntityQueryDesc[] { desc1, desc2, desc3, desc4 });


        }


        protected override void OnUpdate()
        {
            
            
            Entities
                .WithStoreEntityQueryInField(ref _query)
                .ForEach((ref Movement movement, in AnimatorWrapper animator, in Attack attack,  in AnimatorConfiguration conf) =>
                {
                    if (attack.Enable)
                    {
                        animator.Value.SetBool(conf.Attack.ToString(), true);
                    }

                    movement.Enable = false;
                })
                .WithoutBurst()
                .Run();
        }
    }
}