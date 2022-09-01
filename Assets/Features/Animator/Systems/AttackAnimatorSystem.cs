using Features.Animator.Components;
using Features.Character.Components;
using Features.Character.Systems;
using Features.Character.Systems.SystemGroups;
using Features.InputSystem.Systems;
using Unity.Entities;
using UnityEngine;

namespace Features.Animator.Systems
{
    [UpdateInGroup(typeof(GameObjectSyncGroup))]
    public partial class AttackAnimatorSystem : SystemBase
    {
        protected override void OnCreate()
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
        
        protected override void OnUpdate()
        {
            Entities
                .WithAll<AnimatorWrapper, Attack, AnimatorConfiguration>()
                .ForEach((ref Attack attack, in AnimatorWrapper animator, in AnimatorConfiguration conf) =>
                {
                    if (attack.Trigger == false)
                        return;

                    attack.Trigger = false;
                    
                    if (attack.Enable)
                    {
                        animator.Value.SetInteger(conf.AttackId.ToString(), attack.CurrentAttackId);
                    }
                    
                    animator.Value.SetBool(conf.Attack.ToString(), attack.Enable);
                })
                .WithoutBurst()
                .Run();
        }
    }
}