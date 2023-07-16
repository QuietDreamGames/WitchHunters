using Features.Damage.Implementations;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class InquisitorSecondarySolarWave : ASkill
    {
        [SerializeField] private WaveController _waveController;
        
        public override void Cast(Vector3 direction, ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            var range = modifiersContainer.GetValue(ModifierType.SecondarySkillRange,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillRange));
            var damage = modifiersContainer.GetValue(ModifierType.SecondarySkillDamage,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillDamage));
            var speed = modifiersContainer.GetValue(ModifierType.SecondarySkillSpeed,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillSpeed));
            var lifetime = modifiersContainer.GetValue(ModifierType.SecondarySkillLifetime,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillLifetime));
            var maxSize = modifiersContainer.GetValue(ModifierType.SecondarySkillMaxSize,
                baseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillMaxSize));
            
            var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");

            _waveController.Cast(range, damage, speed, lifetime, maxSize, direction,
                new InqSolarSecondaryWaveDamageInstance(hittableLayerMask, obstacleLayerMask, modifiersContainer,
                    baseModifiersContainer, null, transform));
        }
    }
}