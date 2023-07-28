using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Interfaces;
using UnityEngine;

namespace Features.Skills.Core
{
    public class ShieldHealthController : IShieldHealthController
    {
        private float _maxShieldHealth;
        private float _currentShieldHealth;
        private float _shieldRegenRate;
        private float _shieldRegenDelay;
        
        private float _shieldRegenDelayTimer;
        
        private bool _isShieldActive;
        
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        
        
        public ShieldHealthController(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            
            _maxShieldHealth = _modifiersContainer.GetValue(ModifierType.ShieldMaxHealth, baseModifiersContainer.GetBaseValue(ModifierType.ShieldMaxHealth));
            _currentShieldHealth = _maxShieldHealth;
            _shieldRegenRate = _modifiersContainer.GetValue(ModifierType.ShieldRegenRate, baseModifiersContainer.GetBaseValue(ModifierType.ShieldRegenRate));
            _shieldRegenDelay = _modifiersContainer.GetValue(ModifierType.ShieldRegenDelay, baseModifiersContainer.GetBaseValue(ModifierType.ShieldRegenDelay));
            _modifiersContainer.OnUpdateModifier += OnUpdateModifiers;
        }
        
        public void GetShieldHealth(out float currentShieldHealth, out float maxShieldHealth)
        {
            currentShieldHealth = _currentShieldHealth;
            maxShieldHealth = _maxShieldHealth;
        }
        
        public void SetShieldActive(bool isActive)
        {
            _isShieldActive = isActive;
        }
        
        public float GetHit(float damage)
        {
            if (!_isShieldActive) return damage;
            
            _currentShieldHealth -= damage;
            _shieldRegenDelayTimer = _shieldRegenDelay;
            
            var overDamage = 0f;
            
            if (_currentShieldHealth <= 0)
            {
                overDamage = Mathf.Abs(_currentShieldHealth);
                _currentShieldHealth = 0;
            }
            
            return overDamage;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_isShieldActive) return;
            if (_currentShieldHealth >= _maxShieldHealth) return;
            
            if (_shieldRegenDelayTimer > 0)
            {
                _shieldRegenDelayTimer -= deltaTime;
                return;
            }
            
            _currentShieldHealth += _shieldRegenRate * deltaTime;
            _currentShieldHealth = _currentShieldHealth > _maxShieldHealth ? _maxShieldHealth : _currentShieldHealth;
        }

        public void OnUpdateModifiers(ModifierType modifierType)
        {
            switch (modifierType)
            {
                case ModifierType.ShieldMaxHealth:
                    _maxShieldHealth = _modifiersContainer.GetValue(ModifierType.ShieldMaxHealth, _baseModifiersContainer.GetBaseValue(ModifierType.ShieldMaxHealth));
                    break;
                case ModifierType.ShieldRegenRate:
                    _shieldRegenRate = _modifiersContainer.GetValue(ModifierType.ShieldRegenRate, _baseModifiersContainer.GetBaseValue(ModifierType.ShieldRegenRate));
                    break;
                case ModifierType.ShieldRegenDelay:
                    _shieldRegenDelay = _modifiersContainer.GetValue(ModifierType.ShieldRegenDelay, _baseModifiersContainer.GetBaseValue(ModifierType.ShieldRegenDelay));
                    break;
            }
        }
    }
}