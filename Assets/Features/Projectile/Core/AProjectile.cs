using System;
using Features.ColliderController.Core;
using UnityEngine;

namespace Features.Projectile.Core
{
    public abstract class AProjectile : MonoBehaviour
    {
        public RangeColliderInfo rangeColliderInfo;
        
        [SerializeField] protected float speed = 5f;
        [SerializeField] protected float lifeTime = 5f;
        
        public bool isUsed;
        private float _lifeTimeCounter;
        private Vector3 _direction;
        private Transform _initialParent;
        
        public virtual void Fire(Vector3 position, Vector3 direction, float angle)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
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
                _lifeTimeCounter = 0f;
                isUsed = false;
                gameObject.SetActive(false);
                transform.parent = _initialParent;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!isUsed) return;
            // return;
            transform.position += _direction * (speed * Time.fixedDeltaTime);
        }
    }
}