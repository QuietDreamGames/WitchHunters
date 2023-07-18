using System;
using System.Collections;
using UnityEngine;

namespace Features.Dungeons.Cutoff.Scripts
{
    public class CutoffController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [Header("Trigger")]
        [SerializeField] private LayerMask triggerLayerMask;

        [Header("Parameters")] 
        [SerializeField] private Color defaultColor = Color.black;
        
        [Header("Activation")]
        [SerializeField] private int activationStencilRef = 2;
        [SerializeField] private float activationDuration = 1;
        
        [Header("Un Active")]
        [SerializeField] private int unActiveStencilRef = 1;
        [SerializeField] private float unActiveAlpha = 1;
        
        [Header("Active")]
        [SerializeField] private int activeStencilRef = 3;
        [SerializeField] private float activeAlpha = 0;
        
        private MaterialPropertyBlock _materialPropertyBlock;
        
        private bool _isActivated;
        
        private static readonly int StencilRef = Shader.PropertyToID("_StencilRef");
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Start()
        {
            spriteRenderer.enabled = true;
            
            _materialPropertyBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            
            spriteRenderer.material.SetColor(Color1, defaultColor);
            spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
            
            SetUnActive();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActivated)
            {
                return;
            }
            
            var isTrigger = triggerLayerMask == (triggerLayerMask | (1 << other.gameObject.layer));
            if (!isTrigger)
            {
                return;
            }
            
            SetActive();
        }

        private void SetUnActive()
        {
            spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            spriteRenderer.material.SetInt(StencilRef, unActiveStencilRef);
            spriteRenderer.material.SetFloat(Alpha, unActiveAlpha);
            spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
            
            _isActivated = false;
        }
        
        private void SetActive()
        {
            spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            Debug.Log($"Set active stencil ref {_materialPropertyBlock.GetInt(StencilRef)}");
            spriteRenderer.material.SetInt(StencilRef, activationStencilRef);
            spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
            
            _isActivated = true;
            
            StartCoroutine(CSetActive());
        }

        private IEnumerator CSetActive()
        {
            var time = 0f;
            while (time < activationDuration)
            {
                spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
                Debug.Log($"Set active alpha {_materialPropertyBlock.GetFloat(Alpha)}");
                time += Time.deltaTime;
                var alpha = Mathf.Lerp(unActiveAlpha, activeAlpha, time / activationDuration);
                spriteRenderer.material.SetFloat(Alpha, alpha);
                spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
                yield return null;
            }
            
            spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            Debug.Log($"Set active stencil ref {_materialPropertyBlock.GetInt(StencilRef)}");
            spriteRenderer.material.SetInt(StencilRef, activeStencilRef);
            spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
