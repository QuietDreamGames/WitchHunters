﻿using System;
using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageController : MonoBehaviour, IDamageable, IFixedUpdateHandler
    {
        [SerializeField] private HitEffectController _hitEffectController;
        [SerializeField] private HitShaderController _hitShaderController;
        
        [Header("Origin")]
        [SerializeField] private Transform _origin;

        [Header("DEBUG")] 
        [SerializeField] private bool _activeOnStart;

        private HealthComponent _healthComponent;
        private ModifiersContainer _modifiersesController;
        private BaseModifiersContainer _baseModifiersContainer;


        private Vector3 _knockbackForce;
        private float _knockbackDuration;
        private float _knockbackTimer;
        
        public Action<Vector3, HitEffectType> OnAnyHit;
        public Action<Vector3, HitEffectType> OnDamageHit;
        public Action OnDeath;
        
        private bool _isDead;

        private bool _isActive;

        public void Initiate(ModifiersContainer modifiersesController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent)
        {
            _modifiersesController = modifiersesController;
            _healthComponent = healthComponent;
            _baseModifiersContainer = baseModifiersContainer;
            
            if (_origin == null)
            {
                _origin = transform;
            }
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }
        
        public virtual void Restart()
        {
            _isDead = false;
            _healthComponent.Restart();
        }

        public virtual void TakeDamage(float damage, Vector3 forceDirection, HitEffectType hitEffectType)
        {
            if (!_isActive)
            {
                return;
            }
            
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
        
        private void Start()
        {
            if (_activeOnStart)
            {
                SetActive(true);
            }
        }

        private void OnDestroy()
        {
            if (_activeOnStart)
            {
                SetActive(false);
            }
        }
    }
}