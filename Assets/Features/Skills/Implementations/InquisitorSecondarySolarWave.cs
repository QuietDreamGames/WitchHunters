using Features.Damage.Implementations;
using Features.Modifiers;
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
            var speed = ModifiersContainer.GetValue(ModifierType.SecondarySkillSpeed,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillSpeed));
            var lifetime = ModifiersContainer.GetValue(ModifierType.SecondarySkillLifetime,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillLifetime));
            var maxSize = ModifiersContainer.GetValue(ModifierType.SecondarySkillMaxSize,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondarySkillMaxSize));
            
            var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");

            _waveController.Cast(range, speed, lifetime, maxSize, direction,
                new InqSolarSecondaryWaveDamageInstance(hittableLayerMask, obstacleLayerMask, ModifiersContainer,
                    BaseModifiersContainer));
            
            _maxCooldown = ModifiersContainer.GetValue(ModifierType.SecondaryCooldown,
                BaseModifiersContainer.GetBaseValue(ModifierType.SecondaryCooldown));
            _currentCooldown = _maxCooldown;
        }
        
        public override void OnUpdate(float deltaTime)
        {
            _waveController.OnUpdate(deltaTime);
            
            if (IsOnCooldown) _currentCooldown -= deltaTime;
            
            if (_currentCooldown < 0f) _currentCooldown = 0f;
        }
        
        public override void OnFixedUpdate(float deltaTime)
        {
            _waveController.OnFixedUpdate(deltaTime);
        }
    }
}