using System;
using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageController : MonoBehaviour, IDamageable
    {
        private HealthComponent _healthComponent;
        private ModifiersContainer _modifiersesController;
        private BaseModifiersContainer _baseModifiersContainer;

        public Action<Vector3, HitEffectType> OnAnyHit;
        public Action<Vector3, HitEffectType> OnDamageHit;
        public Action OnDeath;
        
        private bool _isDead;

        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent)
        {
            _modifiersesController = modifiersController;
            _healthComponent = healthComponent;
            _baseModifiersContainer = baseModifiersContainer;
        }
        
        public virtual void Restart()
        {
            _isDead = false;
            _healthComponent.Restart();
        }

        public virtual void TakeDamage(float damage, Vector3 forceDirection, HitEffectType hitEffectType)
        {
            if (_isDead) return;
            
            var armor = _modifiersesController.GetValue(ModifierType.Armor,
                _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;

            OnAnyHit?.Invoke(forceDirection, hitEffectType);

            if (damageTaken <= 0) return;
         
            OnDamageHit?.Invoke(forceDirection, hitEffectType);

            var healthAfterDamage = _healthComponent.TakeDamage(damageTaken);
            
            if (healthAfterDamage <= 0.01f)
            {
                OnDeathEvent();
                OnDeath?.Invoke();
            }
        }

        protected virtual void OnDeathEvent()
        {
            _isDead = true;
        }
    }
}