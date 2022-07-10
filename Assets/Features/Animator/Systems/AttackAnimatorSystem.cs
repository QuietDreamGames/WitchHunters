using Features.Animator.Components;
using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Systems;
using Unity.Entities;
using UnityEngine;

namespace Features.Animator.Systems
{
    [UpdateAfter(typeof(AttackSystem))]
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
                .WithAll<AnimatorWrapper, Attack, AnimatorConfiguration>()
                .ForEach((ref Movement movement, in AnimatorWrapper animator, in Attack attack,  in AnimatorConfiguration conf) =>
                {
                    animator.Value.SetBool(conf.Attack.ToString(), attack.Enable);

                    Debug.Log($"{conf.AttackId.ToString()} - {attack.CurrentAttackId}");
                    
                    animator.Value.SetInteger(conf.AttackId.ToString(), attack.CurrentAttackId);
                })
                .WithoutBurst()
                .Run();
        }
    }
}