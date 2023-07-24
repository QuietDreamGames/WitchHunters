using System;
using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.VFX;
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
        public Action<Vector3> OnDeath;

        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent)
        {
            _modifiersesController = modifiersController;
            _healthComponent = healthComponent;
            _baseModifiersContainer = baseModifiersContainer;
        }

        public virtual void TakeDamage(float damage, Vector3 forceDirection, HitEffectType hitEffectType)
        {
            var armor = _modifiersesController.GetValue(ModifierType.Armor, _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;

            if (damageTaken <= 0)
            {
                OnAnyHit?.Invoke(forceDirection, hitEffectType);
            }
            else
            {
                OnDamageHit?.Invoke(forceDirection, hitEffectType);
                if (_healthComponent.TakeDamage(damageTaken) <= 0.01f)
                {
                    OnDeath?.Invoke(forceDirection);
                }
            }
        }
    }
}