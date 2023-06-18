using System;
using Features.Damage.Interfaces;
using Features.Modifiers;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Features.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private ModifiersController _modifiersController;
        
        private float _currentHealth;
        private float _maxHealth;
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Debug.Log("Dead");
            }
        }

        public void TakeDamage(float damage, Vector2 forceDirection)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Debug.Log("Dead");
            }
        }

        private void OnEnable()
        {
            _modifiersController.OnModifierChanged += OnModifiersChanged;
        }

        private void OnDisable()
        {
            _modifiersController.OnModifierChanged -= OnModifiersChanged;
        }

        private void Start()
        {
            _maxHealth = _modifiersController.CalculateModifiedValue(ModifierType.MaximumHealth);
            _currentHealth = _maxHealth;
        }

        private void OnModifiersChanged(ModifierType type)
        {
            if (type != ModifierType.MaximumHealth) return;
            
            _maxHealth = _modifiersController.CalculateModifiedValue(ModifierType.MaximumHealth);
        }
    }
}