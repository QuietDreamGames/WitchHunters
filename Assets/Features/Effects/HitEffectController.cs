using UnityEngine;

namespace Features.Effects
{
    public class HitEffectController : MonoBehaviour
    {
        
        [SerializeField] private ParticleSystem _bloodHitParticleSystem;
        [SerializeField] private ParticleSystem _crossHitParticleSystem;
        
        [SerializeField] private float _crossHitParticleRotationOffset;
        
        public void PlayHitEffect(Vector3 forceDirection)
        {
            // _hitParticleSystem.startRotation = Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg;
            // main.startRotation = 
            
            var randomOffset = Random.Range(-_crossHitParticleRotationOffset, _crossHitParticleRotationOffset);
            _crossHitParticleSystem.transform.rotation = Quaternion.Euler(0, 0,
                (Mathf.Atan2(forceDirection.y, forceDirection.x) + randomOffset) * Mathf.Rad2Deg);
            _bloodHitParticleSystem.transform.rotation = Quaternion.Euler(0, 0,
                Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg);
            _crossHitParticleSystem.Play();
            _bloodHitParticleSystem.Play();
        }
        
        public void PlayHitEffect()
        {
            var randomOffset = Random.Range(-_crossHitParticleRotationOffset, _crossHitParticleRotationOffset);
            _crossHitParticleSystem.transform.rotation = Quaternion.Euler(0, 0, randomOffset);
        }
    }
}