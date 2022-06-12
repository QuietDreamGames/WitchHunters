using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        #region Serialize data

        [Header("Params")]
        [SerializeField] private float _moveSpeed = 1;
        
        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;

        #endregion

        #region Monobehaviour

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;

            var isMoving = math.any(_movementInput != float2.zero);

            if (isMoving)
            {
                _rigidbody.MovePosition(_rigidbody.position + (Vector2)(_movementInput * _moveSpeed * deltaTime));
                
                _animator.SetFloat("Horizontal", _movementInput.x);
                _animator.SetFloat("Vertical", _movementInput.y);
            }
            
            _animator.SetBool("Move", isMoving);
        }

        #endregion
        
        #region Private fields

        private float2 _movementInput;

        #endregion
        
        #region Player Input implementation

        private void OnMove(InputValue value)
        {
            _movementInput = value.Get<Vector2>();
        }

        #endregion
    }
}
