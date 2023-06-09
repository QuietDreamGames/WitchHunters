using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    public class CharacterController : MonoBehaviour
    {
        // I need to have custom Character Controller for 2D character, Use CharacterView for the character's visual representation
        // Somehow get PlayerInput and use it to move the character
        
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Collider2D _collider2D;

        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector2 _lastMovementInput;
        
        private PlayerInput _playerInput;
        private bool _isActive;
        
        public void SetPlayerInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        public void SetActive(bool state)
        {
            _isActive = state;
        }
        
        private void FixedUpdate()
        {
            if (!_isActive) return;
            
            Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                _characterView.PlayWalkAnimation(moveInput);
                _lastMovementInput = moveInput; 
            }
            else
            {
                _characterView.PlayIdleAnimation(_lastMovementInput);
            }
            
            Move(moveInput);
        }

        private void Move(Vector2 moveInput)
        {
            Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
            transform.Translate(movement * (Time.fixedDeltaTime * speed));
        }
    }
}