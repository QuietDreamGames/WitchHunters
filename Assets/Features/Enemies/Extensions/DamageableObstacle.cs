using System;
using System.Collections;
using Features.ObjectPools.Core;
using Features.Test;
using Features.TimeSystems.Interfaces.Handlers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Enemies.Extensions
{
    public class DamageableObstacle : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Space]
        [SerializeField] private float fadeDuration;
        [SerializeField] private float lifeTime;
        
        [Space]
        [SerializeField] private DamageDealAoe damageDealAoe;

        private Coroutine _behaviourCoroutine;
        
        private float _deltaTime;
        
        private bool _isRunning;
        
        public GameObjectPool<DamageableObstacle> Pool { get; set; } = null;
        public GameObject Prefab { get; set; } = null;
        
        public float Damage 
        {
            get => damageDealAoe.Damage; 
            set => damageDealAoe.Damage = value;
        }
        
        public float Delay 
        {
            get => damageDealAoe.Delay; 
            set => damageDealAoe.Delay = value;
        }
        
        public float LifeTime 
        {
            get => lifeTime; 
            set => lifeTime = value;
        }
        
        public LayerMask LayerMask 
        {
            get => damageDealAoe.LayerMask; 
            set => damageDealAoe.LayerMask = value;
        }

        private void Awake()
        {
            SetAlpha(0);
            
            damageDealAoe.SetActive(false);

            _isRunning = false;
        }

        public void Spawn(Vector3 position)
        {
            if (_behaviourCoroutine != null)
            {
                StopCoroutine(_behaviourCoroutine);
                _behaviourCoroutine = null;
            }
            
            _isRunning = true;
            
            SetAlpha(0);
            
            transform.position = position;
            _behaviourCoroutine = StartCoroutine(CBehaviour());
        }
        
        public void Despawn()
        {
            _isRunning = false;
            
            SetAlpha(0);
            
            damageDealAoe.SetActive(false);
            Pool.Despawn(Prefab, this);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }
            
            _deltaTime = deltaTime;
        }
        
        private void SetAlpha(float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        private IEnumerator CBehaviour()
        {
            yield return CFade(fadeDuration, 1);
            yield return CDealDamage(lifeTime);
            yield return CFade(fadeDuration, 0);
            
            Despawn();
        }
        
        private IEnumerator CFade(float duration, float to)
        {
            var alpha = spriteRenderer.color.a;
            
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
        }

        private IEnumerator CDealDamage(float duration)
        {
            damageDealAoe.SetActive(true);
            
            float timer = 0;
            while (timer < duration)
            {
                timer += _deltaTime;
                yield return null;
            } 
            
            damageDealAoe.SetActive(false);
        }
    }
}
