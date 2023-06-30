using Features.Damage.Interfaces;
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
                CalculateDamage();
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
        
        private void CalculateDamage()
        {
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = _hitboxCollider.OverlapCollider(contactFilter2D, colliders);
            
            for (int j = 0; j < colliderCount; j++)
            {
                
                var damageable = colliders[j].GetComponent<IDamageable>();
                if (damageable == null) continue;

                var damage = 50; //_modifiersController.CalculateModifiedValue(ModifierType.AttackDamage);
                // var knockbackDirection = colliders[j].transform.position - transform.position;
                // knockbackDirection.Normalize();
                // var knockbackForce = knockbackDirection * _modifiersController.CalculateModifiedValue(ModifierType.KnockbackForce);
                damageable.TakeDamage(damage);
                
                // _collidersDamaged.Add(colliders[j]);
            }
            
        }
    }
}