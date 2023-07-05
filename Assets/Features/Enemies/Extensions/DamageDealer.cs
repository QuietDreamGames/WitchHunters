using System;
using System.Collections.Generic;
using Features.Damage.Interfaces;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private Collider2D[] colliders;

        private readonly List<Transform> _cachedTargets = new(10);
        private readonly RaycastHit2D[] raycasts = new RaycastHit2D[10];
        
        public void DealDamage(float damage, Transform origin, LayerMask mask)
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
                    if (_cachedTargets.Contains(target))
                    {
                        continue;
                    }
                    
                    var damageable = target.GetComponent<IDamageable>();
                    if (damageable == null)
                    {
                        continue;
                    }
                    
                    var direction = target.position - origin.position;
                    damageable.TakeDamage(damage, direction);
                    
                    if (!_cachedTargets.Contains(target))
                    {
                        _cachedTargets.Add(target);
                    }
                }
            }
        }

        public void Reset()
        {
            _cachedTargets.Clear();
        }
    }
}
