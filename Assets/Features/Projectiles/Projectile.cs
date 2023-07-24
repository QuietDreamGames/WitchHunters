using System;
using System.Collections;
using Features.Damage.Interfaces;
using Features.ObjectPools.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IFixedUpdateHandler
    {
        [SerializeField] private Transform view;
        [SerializeField] private new Collider2D collider;
        
        [Header("Masks")] 
        [SerializeField] private LayerMask damageableMask;
        [SerializeField] private LayerMask obstacleMask;
        
        [Header("Parameters")]
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float lifetime;
        
        protected Vector3 Target;
        
        protected bool IsRunning;
        
        private RaycastHit2D[] _raycasts = new RaycastHit2D[10];
        
        private WaitForSeconds _waitForSeconds;
        private Coroutine _lifetimeDespawn;

        public GameObjectPool<Projectile> Pool { get; set; } = null;
        public GameObject Prefab {get; set; } = null;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }
        
        public float Damage
        {
            get => damage;
            set => damage = value;
        }
        
        public float Lifetime
        {
            get => lifetime;
            set => lifetime = value;
        }

        public void Spawn(Vector3 target)
        {
            if (IsRunning)
            {
                return;
            }
            
            IsRunning = true;
            
            Target = target;
            
            _waitForSeconds = new WaitForSeconds(lifetime);
            _lifetimeDespawn = StartCoroutine(CLifetimeDespawn());
            
            OnSpawn();
        }

        public void Despawn()
        {
            if (!IsRunning)
            {
                return;
            }
            
            IsRunning = false;
            
            OnDespawn();
            
            StopCoroutine(_lifetimeDespawn);
            Pool.Despawn(Prefab, this);
        }
        
        protected abstract void OnSpawn();
        protected abstract void OnDespawn();
        protected abstract void Translate(float deltaTime);
        
        protected void RotateView(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            view.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        public void OnFixedUpdate(float deltaTime)
        {
            if (!IsRunning)
            {
                return;
            }
            
            var isColliding = TryCollision();
            if (isColliding)
            {
                Despawn();
                return;
            }
            
            Translate(deltaTime);
        }

        private bool TryCollision()
        {
            var bounds = collider.bounds;
            var count = Physics2D.BoxCastNonAlloc(
                bounds.center,
                bounds.size,
                0,
                Vector2.zero,
                _raycasts,
                0,
                damageableMask | obstacleMask);

            for (var i = 0; i < count; i++)
            {
                var raycast = _raycasts[i];
                var layer = raycast.collider.gameObject.layer;
                var isDamageableLayer = damageableMask == (damageableMask | (1 << layer));
                if (isDamageableLayer)
                {
                    var damageable = raycast.collider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage, -raycast.normal);
                    }
                }
            }
            
            return count != 0;
        }

        private IEnumerator CLifetimeDespawn()
        {
            yield return _waitForSeconds;
            Despawn();
        }
    }
}
