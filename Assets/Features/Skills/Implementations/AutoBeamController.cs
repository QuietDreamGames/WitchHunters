using Features.Damage.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class AutoBeamController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _beamParticles;
        [SerializeField] private Collider2D _hitboxCollider;
        
        [SerializeField] private float _delayBeforeDamage = 1.5f;
        [SerializeField] private float _delayBeforeDelete = 1.8f;

        private float _beamTimer;
        private bool _hit;
        private AColliderDamageProcessor _damageProcessor; 

        public void Initiate(AColliderDamageProcessor damageProcessor)
        {
            _damageProcessor = damageProcessor;
            _damageProcessor.SetCollider(_hitboxCollider);
        }
        
        public void Cast()
        {
            _beamParticles.Play();
            _beamTimer = 0f;
            _hit = false;
        }

        private void Update()
        {
            if (_beamTimer > _delayBeforeDamage && !_hit)
            {
                _damageProcessor.InstantProcessDamage();
                _hit = true;
            }
            if (_beamTimer > _delayBeforeDelete)
            {
                _beamTimer = 0f;
                enabled = false;
                Destroy(gameObject);
            }
            else
            {
                _beamTimer += Time.deltaTime;
            }
        }
    }
}