using Features.Damage.Core;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Knockback
{
    public class KnockbackController : MonoBehaviour, IFixedUpdateHandler
    {
        [SerializeField] private DamageController _damageController;
        
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        
        private Vector3 _knockbackForce;
        private float _knockbackDuration;
        private float _knockbackTimer;
        
        private Transform _knockingTransform;

        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer,
            Transform knockingTransform = null)
        {
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            
            _knockingTransform = knockingTransform == null ? transform : knockingTransform;
            _damageController.OnAnyHit += OnHit;
        }

        private void OnHit(Vector3 forceDirection, HitEffectType hitEffectType)
        {
            if (forceDirection.magnitude < 0.1f) return;
            
            _knockbackDuration = 0.1f;
            _knockbackTimer = _knockbackDuration;
            var knockbackForceMultiplier = _modifiersContainer.GetValue(ModifierType.KnockbackResistance,
                _baseModifiersContainer.GetBaseValue(ModifierType.KnockbackResistance));
            
            _knockbackForce = forceDirection * knockbackForceMultiplier;
        }
        
        public void OnFixedUpdate(float deltaTime)
        {
            if (_knockbackTimer <= 0) return;
            
            _knockbackTimer -= deltaTime;
            _knockingTransform.Translate(_knockbackForce * deltaTime);
        }
    }
}