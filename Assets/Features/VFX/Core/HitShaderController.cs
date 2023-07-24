using Features.Damage.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.VFX.Core
{
    public class HitShaderController : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private DamageController _damageController;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public float blendSpeed = 5f; // Controls the speed of color blending

        private Material _material;
        private bool _isHit;
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        
        private float _currentFlashAmount;

        private void Start()
        {
            _material = _spriteRenderer.material;
            _damageController.OnDamageHit += PlayHitEffect;
        }

        public void PlayHitEffect(Vector3 forceDirection, HitEffectType hitEffectType)
        {
            _isHit = true;
            _currentFlashAmount = 1f;
            _material.SetFloat(FlashAmount, _currentFlashAmount);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isHit) return;
            
            _currentFlashAmount = Mathf.Lerp(_currentFlashAmount, 0f, blendSpeed * deltaTime);
            
            if (_currentFlashAmount < 0.01f)
            {
                _currentFlashAmount = 0f;
                _isHit = false;
            }
            
            _material.SetFloat(FlashAmount, _currentFlashAmount);
        }
    }
}