using System;
using System.Collections;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI
{
    public class LoadingScreenUI : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Image background;
        
        [Header("Parameters")]
        [SerializeField] private float fadeDuration = 0.5f;
        
        [Header("DEBUG")]
        [SerializeField] private bool showOnStart;
        
        private Coroutine _coroutine;
        
        private float _deltaTime;
        
        private void Start()
        {
            if (showOnStart)
            {
                SetAlpha(1);
                Hide();
            }
        }
        
        public void Show()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            
            _coroutine = StartCoroutine(SetAlphaCoroutine(1));
        }
        
        public void Hide()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            
            _coroutine = StartCoroutine(SetAlphaCoroutine(0));
        }

        public void Reset()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            } 
        }

        public IEnumerator SetAlphaCoroutine(float targetAlpha)
        {
            var startAlpha = background.color.a;
            var time = 0f;
            while (time < fadeDuration)
            {
                time += _deltaTime;
                var alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                SetAlpha(alpha);
                yield return null;
            }
            
            SetAlpha(targetAlpha);
        }
        
        public void SetAlpha(float alpha)
        {
            var color = background.color;
            color.a = alpha;
            background.color = color;
        }

        public void OnUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
        }
    }
}
