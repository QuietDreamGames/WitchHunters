﻿using System;
using System.Collections.Generic;
using Features.Damage;
using Features.Damage.Interfaces;
using Features.Modifiers;
using Features.Team;
using UnityEngine;

namespace Features.ColliderController.Core
{
    public class MeleeColliderController : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private MeleeColliderInfo[] _collidersInfo;
        [SerializeField] private ModifiersController _modifiersController;
        
        private List<Collider2D> _collidersDamaged = new List<Collider2D>();
        
        public void EnableCollider(MeleeColliderType meleeColliderType)
        {
            _collider.enabled = true;
            _collidersDamaged = new List<Collider2D>();
            
            for (int i = 0; i < _collidersInfo.Length; i++)
            {
                if (_collidersInfo[i].meleeColliderType == meleeColliderType)
                {
                    _collider.offset = _collidersInfo[i].offset;
                    _collider.size = _collidersInfo[i].size;
                }
            }
        }

        public void DisableCollider()
        {
            _collider.enabled = false;
        }

        private void Update()
        {
            if (!_collider.enabled) return;
            
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = _collider.OverlapCollider(contactFilter2D, colliders);

            for (int j = 0; j < colliderCount; j++)
            {
                if (_collidersDamaged.Contains(colliders[j])) continue;
                
                var damageable = colliders[j].GetComponent<IDamageable>();
                if (damageable == null) continue;
                
                var damage = _modifiersController.CalculateModifiedValue(ModifierType.AttackDamage);
                var knockbackDirection = colliders[j].transform.position - transform.position;
                knockbackDirection.Normalize();
                var knockbackForce = knockbackDirection * _modifiersController.CalculateModifiedValue(ModifierType.KnockbackForce);
                damageable.TakeDamage(damage, knockbackForce);
                
                _collidersDamaged.Add(colliders[j]);
            }
        }
    }
}