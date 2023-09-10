﻿using System;
using System.Collections.Generic;
using Features.Damage.Core;
using Features.Damage.Interfaces;
using Features.ServiceLocators.Core;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Test
{
    public class DamageDealAoe : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private float _delay;
        [SerializeField] private LayerMask _layerMask;
        
        private List<Collider2D> _collidersDamaged = new List<Collider2D>();
        
        private DamageableCache _damageableCache;

        private float _timer;

        private void Awake()
        {
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();
        }

        private void DealDamage()
        {
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            contactFilter2D.useLayerMask = true;
            contactFilter2D.SetLayerMask(_layerMask);
            int colliderCount = _collider2D.OverlapCollider(contactFilter2D, colliders);

            for (int i = 0; i < colliderCount; i++)
            {
                if (_collidersDamaged.Contains(colliders[i])) continue;

                var damageable = _damageableCache.GetDamageable(colliders[i].transform);
                if (damageable == null) continue;

                damageable.TakeDamage(_damage, Vector3.zero, HitEffectType.PhysicalAOE);
                _collidersDamaged.Add(colliders[i]);
            }
        }
        
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _delay)
            {
                _collidersDamaged.Clear();
                DealDamage();
                _timer = 0;
            }
        }
    }
}