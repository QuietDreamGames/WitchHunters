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
        
        public override void Cast(Vector3 direction)
        {
            var range = ModifiersContainer.GetValue(ModifierType.SecondarySkillRange,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillRange));
            var damage = ModifiersContainer.GetValue(ModifierType.SecondarySkillDamage,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillDamage));
            var speed = ModifiersContainer.GetValue(ModifierType.SecondarySkillSpeed,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillSpeed));
            var lifetime = ModifiersContainer.GetValue(ModifierType.SecondarySkillLifetime,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillLifetime));
            var maxSize = ModifiersContainer.GetValue(ModifierType.SecondarySkillMaxSize,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillMaxSize));
            
            var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");

            _waveController.Cast(range, damage, speed, lifetime, maxSize, direction,
                new InqSolarSecondaryWaveDamageInstance(hittableLayerMask, obstacleLayerMask, ModifiersContainer,
                    BaseModifiersContainer, null, transform));
        }
    }
}