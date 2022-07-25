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
        protected override void OnUpdate()
        {
            Entities
                .WithAll<AnimatorWrapper, Attack, AnimatorConfiguration>()
                .ForEach((in AnimatorWrapper animator, in Attack attack,  in AnimatorConfiguration conf) =>
                {
                    animator.Value.SetBool(conf.Attack.ToString(), attack.Enable);
                    animator.Value.SetInteger(conf.AttackId.ToString(), attack.CurrentAttackId);
                })
                .WithoutBurst()
                .Run();
        }
    }
}