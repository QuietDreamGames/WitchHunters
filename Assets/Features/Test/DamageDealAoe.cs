using System.Collections.Generic;
using Features.Damage.Core;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Interfaces.Handlers;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Test
{
    public class DamageDealAoe : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private float _damage;
        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private float _delay;
        [SerializeField] private LayerMask _layerMask;
        
        [Header("DEBUG")]
        [SerializeField] private bool _onAwake;
        
        private List<Collider2D> _collidersDamaged = new();
        
        private DamageableCache _damageableCache;

        private float _timer;
        
        private bool _isRunning;

        public float Damage 
        {
            get => _damage; 
            set => _damage = value;
        }
        
        public float Delay 
        {
            get => _delay; 
            set => _delay = value;
        }
        
        public LayerMask LayerMask 
        {
            get => _layerMask; 
            set => _layerMask = value;
        }

        private void Awake()
        {
            _damageableCache = ServiceLocator.Resolve<DamageableCache>();
            
            if (_onAwake)
            {
                SetActive(true);
            }
        }

        private void OnDestroy()
        {
            if (_onAwake)
            {
                SetActive(false);
            }
        }

        public void SetActive(bool active)
        {
            _isRunning = active;
            _timer = 0;
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

        public void OnUpdate(float deltaTime)
        {
            if (_isRunning == false) 
                return;

            _timer -= deltaTime;
            if (_timer <= 0)
            {
                _collidersDamaged.Clear();
                DealDamage();
                _timer = Delay;
            } 
        }
    }
}