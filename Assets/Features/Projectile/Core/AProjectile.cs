using System;
using System.Collections.Generic;
using Features.ColliderController.Core;
using Features.Team;
using UnityEngine;

namespace Features.Projectile.Core
{
    public abstract class AProjectile : MonoBehaviour
    {
        public RangeColliderInfo rangeColliderInfo;
        
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected float speed = 5f;
        [SerializeField] protected float lifeTime = 5f;
        
        public bool isUsed;
        private float _lifeTimeCounter;
        private Vector3 _direction;
        private Transform _initialParent;
        
        private int _piercingTimes;

        private List<Collider2D> _collidersDamaged; 
        
        public virtual void Fire(Vector3 position, Vector3 direction, float angle)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            _collidersDamaged = new List<Collider2D>();
            _piercingTimes = 0;
            
            gameObject.SetActive(true);
            _initialParent = transform.parent;
            transform.parent = null;
            
            _direction = direction;
        }

        protected virtual void Update()
        {
            if (!isUsed) return;
            
            _lifeTimeCounter += Time.deltaTime;
            if (_lifeTimeCounter >= lifeTime)
            {
                Disable();
            }
            
            var colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = _collider.OverlapCollider(contactFilter2D, colliders);
            for (int i = 0; i < colliderCount; i++)
            {
                ProcessCollider(colliders[i]);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!isUsed) return;
            // return;
            transform.position += _direction * (speed * Time.fixedDeltaTime);
        }

        protected virtual void Disable()
        {
            _lifeTimeCounter = 0f;
            isUsed = false;
            gameObject.SetActive(false);
            transform.parent = _initialParent;
            _piercingTimes = 0;
        }

        protected virtual void ProcessCollider(Collider2D other)
        {
            // var teamComponent = other.GetComponent<TeamComponent>();
            //
            // if (teamComponent == null) return;
            // if (teamComponent.TeamIndex != TeamIndex.Enemy) return;
            if (_collidersDamaged.Contains(other)) return;
                
            _collidersDamaged.Add(other);
            Debug.Log($"fake range damaged {other.gameObject}");
            _piercingTimes += 1;
            if (_piercingTimes >= rangeColliderInfo.basePiercingTimes)
            {
                Disable();
            }
        }
    }
}