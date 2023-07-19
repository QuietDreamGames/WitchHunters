using System;
using UnityEngine;

namespace Features.VFX
{
    public class DestructiblePieceController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _forceMultiplier = 3f;
        [SerializeField] private float _duration = 3f;
        
        private Transform _parentTransform;
        private Vector3 _startLocalPosition;
        private float _timer;

        private void OnValidate()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initiate(Transform parentTransform)
        {
            _parentTransform = parentTransform;
            _startLocalPosition = transform.localPosition;
        }
        
        public void PlayDestructiblePieceEffect()
        {
            transform.parent = _parentTransform;
            transform.localPosition = _startLocalPosition;
            transform.rotation = Quaternion.identity;
            transform.parent = null;
            _rigidbody2D.velocity = Vector2.zero;
            
            var shieldForce = new Vector2
            {
                x = (transform.position.x - _parentTransform.position.x),
                y = (transform.position.y - _parentTransform.position.y)
            };
            
            gameObject.SetActive(true);
            _rigidbody2D.AddForce(shieldForce * _forceMultiplier, ForceMode2D.Impulse);
            _timer = _duration;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_timer <= 0f) return;
            
            _timer -= deltaTime;
            
            _spriteRenderer.color = new Color(1f, 1f, 1f, _timer / _duration);
            
            if (_timer <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}