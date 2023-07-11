using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Team;
using Features.VFX;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageController : MonoBehaviour, IDamageable
    {
        [SerializeField] private HitEffectController _hitEffectController;
        [SerializeField] private HitShaderController _hitShaderController;

        private HealthComponent _healthComponent;
        private ModifiersContainer _modifiersesController;
        private BaseModifiersContainer _baseModifiersContainer;

        public TeamIndex TeamIndex { get; private set; }

        private Vector3 _knockbackForce;
        private float _knockbackDuration;
        private float _knockbackTimer;

        public void Initiate(ModifiersContainer modifiersesController, BaseModifiersContainer baseModifiersContainer, HealthComponent healthComponent, TeamIndex teamIndex)
        {
            _modifiersesController = modifiersesController;
            _healthComponent = healthComponent;
            _baseModifiersContainer = baseModifiersContainer;
            TeamIndex = teamIndex;
        }

        public void TakeDamage(float damage)
        {
            var armor = _modifiersesController.GetValue(ModifierType.Armor, _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;
            if (damageTaken > 0)
            {
                _healthComponent.TakeDamage(damageTaken);
                _hitEffectController.PlayHitEffect();
                _hitShaderController.PlayHitEffect();
            }
        }

        public void TakeDamage(float damage, Vector3 forceDirection)
        {
            var armor = _modifiersesController.GetValue(ModifierType.Armor, _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;
            if (damageTaken > 0)
            {
                _healthComponent.TakeDamage(damageTaken);
                _knockbackForce = forceDirection * (_modifiersesController.GetValue(ModifierType.KnockbackResistance,
                    _baseModifiersContainer.GetBaseValue(ModifierType.KnockbackResistance)) * 3);
                _knockbackDuration = 0.1f;
                _knockbackTimer = _knockbackDuration;
                _hitEffectController.PlayHitEffect(forceDirection);
                _hitShaderController.PlayHitEffect();
            }
        }
        
        private void Update()
        {
            if (_knockbackTimer > 0)
            {
                _knockbackTimer -= Time.deltaTime;
                
                transform.Translate(_knockbackForce * Time.deltaTime);
                
                if (_knockbackTimer <= 0)
                {
                    _knockbackForce = Vector3.zero;
                }
            }
        }
    }
}