using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Implementations
{
    public class InqSolarSecondaryWaveDamageInstance : AColliderDamageProcessor
    {
        public InqSolarSecondaryWaveDamageInstance(LayerMask hittableLayerMask, LayerMask obstacleLayerMask,
            ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer,
            APassiveController passiveController = null, Transform attackerTransform = null) : base(hittableLayerMask,
            obstacleLayerMask, modifiersContainer, baseModifiersContainer, passiveController, attackerTransform)
        {
        }

        protected override void ProcessDamage(IDamageable damageable, Vector3 damageablePosition)
        {
            var damage = modifiersContainer.GetValue(ModifierType.SecondarySkillDamage,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillDamage));
            
            damageable.TakeDamage(damage, Vector3.zero, HitEffectType.FlameRanged);
        }
    }
}