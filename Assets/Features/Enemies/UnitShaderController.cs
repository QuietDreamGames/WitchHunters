using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies
{
    public class UnitShaderController : MonoBehaviour
    {
        #region Serializable data

        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Fade In/Out")]
        [SerializeField] private string alphaProperty = "_Alpha";
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.5f;
        
        [Header("Hit Effect")]
        [SerializeField] private string hitTintProperty = "_HitTint";
        [SerializeField] private Color hitTint = Color.white;
        
        [Space]
        [SerializeField] private string hitProgressProperty = "_HitProgress";
        [SerializeField] private float hitProgressDuration = 0.5f;

        [Space] 
        [SerializeField] private AnimationCurve hitProgressCurve;

        #endregion

        #region Private fields

        private Coroutine _fadeCoroutine;
        private Coroutine _hitCoroutine;
        
        private MaterialPropertyBlock _mpb;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _mpb = new MaterialPropertyBlock();
        }

        #endregion

        #region Public methods

        public void FadeIn(bool instant = false)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }
            
            if (instant)
            {
                SetAlpha(1);
            }
            else
            {
                _fadeCoroutine = StartCoroutine(CFade(fadeInDuration, 1));
            }
        }
        
        public void FadeOut(bool instant = false)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }
            
            if (instant)
            {
                SetAlpha(0);
            }
            else
            {
                _fadeCoroutine = StartCoroutine(CFade(fadeOutDuration, 0));
            }
        }
        
        public void Hit()
        {
            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
            }
            
            _hitCoroutine = StartCoroutine(CHit());
        }

        #endregion

        #region Private methods

        private IEnumerator CHit()
        {
            spriteRenderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(hitTintProperty, hitTint);
            _mpb.SetFloat(hitProgressProperty, 0);
            spriteRenderer.SetPropertyBlock(_mpb);
            
            float timer = 0;
            while (timer < hitProgressDuration)
            {
                timer += Time.deltaTime;
                var progress = timer / hitProgressDuration;
                var curveProgress = hitProgressCurve.Evaluate(progress);
                _mpb.SetFloat(hitProgressProperty, curveProgress);
                spriteRenderer.SetPropertyBlock(_mpb);
                yield return null;
            }
            
            _mpb.SetFloat(hitProgressProperty, 0);
            spriteRenderer.SetPropertyBlock(_mpb);
        }
        
        private void SetAlpha(float alpha)
        {
            spriteRenderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(alphaProperty, alpha);
            spriteRenderer.SetPropertyBlock(_mpb);
        }

        private IEnumerator CFade(float duration, float to)
        {
            spriteRenderer.GetPropertyBlock(_mpb);
            var alpha = _mpb.GetFloat(alphaProperty);
            _mpb.SetColor(hitTintProperty, hitTint);
            _mpb.SetFloat(hitProgressProperty, 0);
            spriteRenderer.SetPropertyBlock(_mpb);
            
            float timer = 0;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var progress = timer / duration;
                var alphaProgress = math.lerp(alpha, to, progress);
                SetAlpha(alphaProgress);
                yield return null;
            }
            
            SetAlpha(to);
        }

        #endregion
        
        
    }
}
