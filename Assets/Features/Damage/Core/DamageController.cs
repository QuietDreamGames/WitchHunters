using Features.Damage.Interfaces;
using Features.Effects;
using Features.Health;
using Features.Modifiers;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageController : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ModifiersController _modifiersController;
        [SerializeField] private HitEffectController _hitEffectController;
        [SerializeField] private HitShaderController _hitShaderController;
        
        private Vector3 _knockbackForce;
        private float _knockbackDuration;
        private float _knockbackTimer;

        public void TakeDamage(float damage)
        {
            var armor = _modifiersController.CalculateModifiedValue(ModifierType.Armor);
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
            var armor = _modifiersController.CalculateModifiedValue(ModifierType.Armor);
            var damageTaken = damage - armor;
            if (damageTaken > 0)
            {
                _healthComponent.TakeDamage(damageTaken);
                _knockbackForce = forceDirection * (_modifiersController.CalculateModifiedValue(ModifierType.KnockbackResistance) * 3);
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