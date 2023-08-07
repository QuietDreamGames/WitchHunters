using System;
using System.Collections.Generic;
using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private Collider2D[] colliders;

        private readonly List<Transform> _cachedTargets = new(10);
        private readonly RaycastHit2D[] raycasts = new RaycastHit2D[10];
        
        private DamageableCache _damageableCache;
        
        public float Damage { get; set; }
        public float KnockbackForce { get; set; }

        public void DealDamage(Transform origin, LayerMask mask)
        {
            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (!collider.enabled)
                {
                    continue;
                }
                
                var bounds = collider.bounds;
                var count = Physics2D.BoxCastNonAlloc(
                    bounds.center, 
                    bounds.size, 
                    0, 
                    Vector2.zero, 
                    raycasts, 
                    0, 
                    mask);

                for (var j = 0; j < count; j++)
                {
                    var target = raycasts[j].transform;
                    var targetCollider = raycasts[j].collider;
                    if (_cachedTargets.Contains(target))
                    {
                        continue;
                    }
                    
                    var damageable = _damageableCache.GetDamageable(target);
                    if (damageable == null)
                    {
                        continue;
                    }
                    
                    var direction = target.position - origin.position;
                    direction = direction.normalized * KnockbackForce;
                    damageable.TakeDamage(Damage, direction);
                    
                    if (!_cachedTargets.Contains(target))
                    {
                        _cachedTargets.Add(target);
                    }
                }
            }
        }

        public void Reset()
        {
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();
            _cachedTargets.Clear();
        }
    }
}
