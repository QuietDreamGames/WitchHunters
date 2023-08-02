﻿using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Implementations
{
    public class InqSolarUltDamageInstance : AColliderDamageProcessor
    {
        public InqSolarUltDamageInstance(LayerMask hittableLayerMask,
            LayerMask obstacleLayerMask, ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, APassiveController passiveController = null)
            : base(hittableLayerMask, obstacleLayerMask, modifiersContainer, baseModifiersContainer, passiveController)
        {
        }

        protected override void ProcessDamage(IDamageable damageable, Vector3 damageablePosition)
        {
            var damage = modifiersContainer.GetValue(ModifierType.UltimateDamage,
                baseModifiersContainer.GetBaseValue(ModifierType.UltimateDamage));
            damageable.TakeDamage(damage, Vector3.zero, HitEffectType.FlameAOE);
        }
    }
}