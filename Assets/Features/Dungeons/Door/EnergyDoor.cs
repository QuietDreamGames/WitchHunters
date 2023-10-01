using System;
using System.Collections;
using Features.Activator;
using Features.Activator.Core;
using Features.TimeSystems.Interfaces.Handlers;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Dungeons.Door
{
    public class EnergyDoor : MonoBehaviour, IUpdateHandler, IActivator
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D coll2D;
        
        [Space]
        [SerializeField] private float fadeTime = 1;
        
        [Space]
        [SerializeField] private float activeAlpha = 1;
        [SerializeField] private float inactiveAlpha = 0;
        
        [Space]
        [SerializeField] private bool activeByDefault = false;
        
        private Coroutine _fadeCoroutine;
        
        private float _deltaTime;

        private void Start()
        {
            if (activeByDefault)
            {
                SetAlpha(activeAlpha);
                coll2D.enabled = true;
            }
            else
            {
                SetAlpha(inactiveAlpha);
                coll2D.enabled = false;
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
        }

        public void Activate()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            coll2D.enabled = true;
            _fadeCoroutine = StartCoroutine(CFade(fadeTime, activeAlpha));
        }

        public void Deactivate()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            coll2D.enabled = false;
            _fadeCoroutine = StartCoroutine(CFade(fadeTime, inactiveAlpha));
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
        
        private void SetAlpha(float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
