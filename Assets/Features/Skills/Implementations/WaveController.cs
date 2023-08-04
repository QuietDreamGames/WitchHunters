﻿using System.Collections.Generic;
using Features.Damage.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider2D;
        
        private float _range;
        private float _speed;
        private float _lifetime;
        private float _maxSize;
        private AColliderDamageProcessor _damageProcessor;
        
        private Vector3 _direction;
        private float _timer;
        private float _currentSize;
        private static readonly int Cast1 = Animator.StringToHash("Cast");

        private bool _isActive;
        
        public void Cast(float range, float speed, float lifetime, float maxSize, Vector3 direction, AColliderDamageProcessor damageProcessor)
        {
            _range = range;
            _speed = speed;
            _lifetime = lifetime;
            _maxSize = maxSize;
            _direction = direction;
            _damageProcessor = damageProcessor;
            _timer = 0f;
            _currentSize = _maxSize / 2f;
            
            transform.localScale = new Vector3(_currentSize, _currentSize, 1);
            transform.localPosition = new Vector3(0, 0, 0);
            
            transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, _direction, Vector3.forward));
            _animator.SetTrigger(Cast1);
            _damageProcessor.SetCollider(_collider2D);
            _damageProcessor.Start();
            _isActive = true;
            
        }

        public void OnUpdate(float deltaTime)
        {
            _animator.Update(deltaTime);
            if (!_isActive) return;
            
            _timer += deltaTime;
            if (_timer > _lifetime)
            {
                _damageProcessor.Stop();
                _isActive = false;
            }
            else
            {
                _currentSize = Mathf.Lerp(_maxSize / 2f, _maxSize, _timer / _lifetime);
                transform.localScale = new Vector3(_currentSize, _currentSize, 1);
                
            }
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!_isActive) return;
            transform.position += _direction * (_speed * deltaTime);
            _damageProcessor.OnFixedUpdate(Time.fixedDeltaTime);
            
        }
    }
}