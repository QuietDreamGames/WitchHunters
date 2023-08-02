using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Implementations
{
    public class InqSolarAttackDamageInstance : AttackDamageInstance
    {
        public InqSolarAttackDamageInstance(LayerMask hittableLayerMask,
            LayerMask obstacleLayerMask, ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, APassiveController passiveController = null) : 
            base( hittableLayerMask, obstacleLayerMask, modifiersContainer, baseModifiersContainer, passiveController)
        {
        }

        protected override void ProcessDamage(IDamageable damageable, Vector3 damageablePosition)
        {
            base.ProcessDamage(damageable, damageablePosition);
            
            if (passiveController.CurrentPassiveInfo.IsCharged)
            {
                var chargedDamage = modifiersContainer.GetValue(ModifierType.PassiveChargedAttackDamage,
                    baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargedAttackDamage));
                damageable.TakeDamage(chargedDamage,  attackDirection / 10 , HitEffectType.FlameMelee);
            }
            
            passiveController.OnHit();
        }
    }
}