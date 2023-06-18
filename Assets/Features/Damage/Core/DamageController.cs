using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageController : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ModifiersController _modifiersController;
        
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