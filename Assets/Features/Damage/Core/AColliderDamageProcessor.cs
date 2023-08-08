using System;
using System.Collections.Generic;
using Features.Damage.Interfaces;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ServiceLocators.Core;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Damage.Core
{
    public abstract class AColliderDamageProcessor
    {
        protected Collider2D collider2D;
        protected LayerMask hittableLayerMask;
        protected LayerMask obstacleLayerMask;
        protected ModifiersContainer modifiersContainer;
        protected BaseModifiersContainer baseModifiersContainer;
        protected APassiveController passiveController;
        protected Vector3 attackDirection;

        private bool _isActive;
        private List<Collider2D> _collidersDamaged;
        
        private DamageableCache _damageableCache;

        public Action OnHitObstacle;
        
        public AColliderDamageProcessor( LayerMask hittableLayerMask, LayerMask obstacleLayerMask,
            ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, APassiveController passiveController = null)
        {
            this.hittableLayerMask = hittableLayerMask;
            this.obstacleLayerMask = obstacleLayerMask;
            this.modifiersContainer = modifiersContainer;
            this.baseModifiersContainer = baseModifiersContainer;
            this.passiveController = passiveController;
        }
        
        public void SetCollider(Collider2D collider2D)
        {
            this.collider2D = collider2D;
        }
        
        public void SetDirection(Vector3 attackDirection)
        {
            this.attackDirection = attackDirection;
        }

        public void Start()
        {
            _isActive = true;
            _collidersDamaged = new List<Collider2D>();
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();
        }
        
        public void Stop()
        {
            _isActive = false;
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (!_isActive) return;
            
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(hittableLayerMask);
            contactFilter2D.useTriggers = true;
            // use SetCollider to set collider2D if you see a null ref here
            int colliderCount = collider2D.OverlapCollider(contactFilter2D, colliders);

            for (int j = 0; j < colliderCount; j++)
            {
                if (_collidersDamaged.Contains(colliders[j])) continue;
                var target = colliders[j].transform;
                if (target == null)
                    continue;
                var damageable = _damageableCache.GetDamageable(target);
                if (damageable == null) continue;
                _collidersDamaged.Add(colliders[j]);
                ProcessDamage(damageable, colliders[j].transform.position);
            }
        }

        public void InstantProcessDamage()
        {
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(hittableLayerMask);
            contactFilter2D.useTriggers = true;
            int colliderCount = collider2D.OverlapCollider(contactFilter2D, colliders);

            for (int j = 0; j < colliderCount; j++)
            {
                var target = colliders[j].transform;
                if (target == null)
                    continue;
                var damageable = _damageableCache.GetDamageable(target);
                if (damageable == null) continue;
                ProcessDamage(damageable, colliders[j].transform.position);
            }
        }

        protected abstract void ProcessDamage(IDamageable damageable, Vector3 damageablePosition);
    }
}