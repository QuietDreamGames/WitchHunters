﻿using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.VFX;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Implementations
{
    public class AttackDamageInstance : AColliderDamageProcessor
    {
        public AttackDamageInstance(LayerMask hittableLayerMask, LayerMask obstacleLayerMask,
            ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer,
            APassiveController passiveController = null, Transform attackerTransform = null) : base(
            hittableLayerMask, obstacleLayerMask, modifiersContainer, baseModifiersContainer, passiveController,
            attackerTransform)
        {
        }

        protected override void ProcessDamage(IDamageable damageable, Vector3 damageablePosition)
        {
            var knockbackDirection = damageablePosition - attackerTransform.position;
            knockbackDirection.Normalize();
            var damage = modifiersContainer.GetValue(ModifierType.AttackDamage,
                baseModifiersContainer.GetBaseValue(ModifierType.AttackDamage));

            var knockbackForce = knockbackDirection * modifiersContainer.GetValue(ModifierType.KnockbackForce,
                baseModifiersContainer.GetBaseValue(ModifierType.KnockbackForce));
            damageable.TakeDamage(damage, knockbackForce, HitEffectType.Physical);
        }

        
    }
}