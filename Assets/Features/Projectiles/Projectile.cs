using System;
using System.Collections;
using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IUpdateHandler, IFixedUpdateHandler
    {
        [SerializeField] private Transform view;
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private new Collider2D collider;
        
        [Header("Masks")] 
        [SerializeField] private LayerMask damageableMask;
        [SerializeField] private LayerMask obstacleMask;
        
        [Header("Parameters")]
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float lifetime;
        
        [Space]
        [SerializeField] private float fadeDuration = 0.5f;
        
        protected Transform Target;
        
        protected bool IsRunning;
        
        private RaycastHit2D[] _raycasts = new RaycastHit2D[10];
        
        private DamageableCache _damageableCache;

        private float _lifetimeDelay;
        private WaitUntil _waitLifetime;
        private Coroutine _lifetimeDespawn;
        
        private float _deltaTime;

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

        public void Spawn(Transform target)
        {
            if (IsRunning)
            {
                return;
            }
            
            IsRunning = true;
            
            Target = target;
            
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();

            _lifetimeDelay = lifetime;
            _waitLifetime = new WaitUntil(() => _lifetimeDelay < 0);
            _lifetimeDespawn = StartCoroutine(CLifetimeDespawn());
            
            SetAlpha(1);
            
            OnSpawn();
        }

        public void Despawn()
        {
            if (!IsRunning)
            {
                return;
            }
            
            IsRunning = false;
            
            StopCoroutine(_lifetimeDespawn);
            StartCoroutine(CFadeDespawn(fadeDuration));
        }
        
        protected abstract void OnSpawn();
        protected abstract void OnDespawn();
        protected abstract void Translate(float deltaTime);
        
        protected void RotateView(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            view.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (!IsRunning)
            {
                return;
            }
            
            _deltaTime = deltaTime;
            
            _lifetimeDelay -= deltaTime;
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

            var isColliding = false;
            
            for (var i = 0; i < count; i++)
            {
                var raycast = _raycasts[i];
                var layer = raycast.collider.gameObject.layer;
                var isDamageableLayer = damageableMask == (damageableMask | (1 << layer));
                if (isDamageableLayer)
                {
                    var damageable = _damageableCache.GetDamageable(raycast.collider.transform);
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage, -raycast.normal);
                        isColliding = true;
                    }
                }
                else
                {
                    isColliding = true;
                }
            }

            return isColliding;
        }
        
        private float GetAlpha()
        {
            return renderer.color.a;
        }
        
        private void SetAlpha(float alpha)
        {
            var rendererColor = renderer.color;
            rendererColor.a = alpha;
            renderer.color = rendererColor;
        }
        
        private IEnumerator CFadeDespawn(float duration)
        {
            var alpha = GetAlpha();
            const int to = 0;
            float timer = 0;
            while (timer < duration)
            {
                timer += _deltaTime;
                var progress = timer / duration;
                var alphaProgress = math.lerp(alpha, to, progress);
                SetAlpha(alphaProgress);
                yield return null;
            }
            
            SetAlpha(to);
            
            OnDespawn();
            Pool.Despawn(Prefab, this);
        }

        private IEnumerator CLifetimeDespawn()
        {
            yield return _waitLifetime;
            
            Despawn();
        }
    }
}
