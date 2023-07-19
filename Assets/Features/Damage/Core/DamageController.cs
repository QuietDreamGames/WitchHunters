using Features.Damage.Interfaces;
using Features.Health;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX;
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

        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
        private bool _isActive;

        public void Initiate(ModifiersContainer modifiersesController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent)
        {
            _modifiersesController = modifiersController;
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

        public virtual void TakeDamage(float damage, Vector3 forceDirection)
        {
            if (!_isActive)
            {
                return;
            }
            
            var armor = _modifiersesController.GetValue(ModifierType.Armor, _baseModifiersContainer.GetBaseValue(ModifierType.Armor));
            var damageTaken = damage - armor;
            if (damageTaken <= 0) return;
            
            _healthComponent.TakeDamage(damageTaken);
            if (_hitShaderController != null)
            {
                _hitShaderController.PlayHitEffect();
            }
            
            if (forceDirection.magnitude < 0.1f) return;
            
            _knockbackForce = forceDirection * (_modifiersesController.GetValue(ModifierType.KnockbackResistance,
                _baseModifiersContainer.GetBaseValue(ModifierType.KnockbackResistance)) * 3);
            _knockbackDuration = 0.1f;
            _knockbackTimer = _knockbackDuration;
            if (_hitEffectController != null)
            {
                _hitEffectController.PlayHitEffect(forceDirection);
            }
            
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }
            
            if (_knockbackTimer > 0)
            {
                _knockbackTimer -= deltaTime;
                
                _origin.Translate(_knockbackForce * deltaTime);
                
                if (_knockbackTimer <= 0)
                {
                    _knockbackForce = Vector3.zero;
                }
            }
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

        public void OnFixedUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);
        }
    }
}