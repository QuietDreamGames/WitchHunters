using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
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


        private Vector3 _knockbackForce;
        private float _knockbackDuration;
        private float _knockbackTimer;

        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent)
        {
            _modifiersesController = modifiersController;
            _healthComponent = healthComponent;
            _baseModifiersContainer = baseModifiersContainer;
        }

        public virtual void TakeDamage(float damage, Vector3 forceDirection)
        {
            var armor = _modifiersesController.GetValue(ModifierType.Armor, _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;
            if (damageTaken <= 0) return;
            
            _healthComponent.TakeDamage(damageTaken);
            _hitShaderController.PlayHitEffect();
            
            if (forceDirection.magnitude < 0.1f) return;
            
            _knockbackForce = forceDirection * (_modifiersesController.GetValue(ModifierType.KnockbackResistance,
                _baseModifiersContainer.GetBaseValue(ModifierType.KnockbackResistance)) * 3);
            _knockbackDuration = 0.1f;
            _knockbackTimer = _knockbackDuration;
            _hitEffectController.PlayHitEffect(forceDirection);
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_knockbackTimer > 0)
            {
                _knockbackTimer -= deltaTime;
                
                transform.Translate(_knockbackForce * deltaTime);
                
                if (_knockbackTimer <= 0)
                {
                    _knockbackForce = Vector3.zero;
                }
            }
        }
    }
}