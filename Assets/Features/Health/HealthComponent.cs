﻿using System;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Features.Health
{
    public class HealthComponent
    {
        private readonly ModifiersContainer _modifiersContainer;
        private readonly BaseModifiersContainer _baseModifiersContainer;
        
        private float _currentHealth;
        private float _maxHealth;

        public Action OnHit;
        public Action OnDeath;

        public HealthComponent(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            
            _modifiersContainer.OnUpdateModifier += OnModifiersChanged;
            _maxHealth = _modifiersContainer.GetValue(ModifierType.MaximumHealth, _baseModifiersContainer.GetBaseValue(ModifierType.MaximumHealth));
            _currentHealth = _maxHealth;
        }

        public float TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0; 
                OnDeath?.Invoke();
                
                return _currentHealth;
            }
            
            OnHit?.Invoke();

            return _currentHealth;
        }

        private void OnModifiersChanged(ModifierType type)
        {
            if (type != ModifierType.MaximumHealth) return;
            
            var oldMaxHealth = _maxHealth;
            
            _maxHealth = _modifiersContainer.GetValue(ModifierType.MaximumHealth, _baseModifiersContainer.GetBaseValue(ModifierType.MaximumHealth));
            RecalculateCurrentHealth(oldMaxHealth);
        }

        private void RecalculateCurrentHealth(float oldMaxHealth)
        {
            var healthPercentage = _currentHealth / oldMaxHealth;
            _currentHealth = _maxHealth * healthPercentage;
        }
        
        public void Restart()
        {
            _currentHealth = _maxHealth;
        }
    }
}