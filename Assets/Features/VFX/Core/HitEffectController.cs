using Features.Damage.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.VFX.Core
{
    public class HitEffectController : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private DamageController _damageController;
        
        [SerializeField] private ParticleSystem _substanceHitParticleSystem;
        [SerializeField] private HitEffectType _hitEffectType;

        private bool _isPlaying;
        private float _timePlaying;
        private float _timer;

        private void Start()
        {
            _damageController.OnDamageHit += PlayHitEffect;
        }

        public void PlayHitEffect(Vector3 forceDirection, HitEffectType hitEffectType)
        {
            if (hitEffectType != _hitEffectType) return;
                
            _substanceHitParticleSystem.transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg);
            _timePlaying = _substanceHitParticleSystem.main.duration;
            _timer = _timePlaying;
            _substanceHitParticleSystem.Simulate(0.0f, true, true);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timer <= 0) return;
            
            _timer -= deltaTime;
            _substanceHitParticleSystem.Simulate(deltaTime, true, false);
        }
    }
}