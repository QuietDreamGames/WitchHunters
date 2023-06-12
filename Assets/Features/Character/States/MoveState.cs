using Features.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class MoveState : State
    {
        private CharacterView _characterView;
        private PlayerInput _playerInput;
        private Transform _transform;
        
        private float _speed = 5f;
        private Vector2 _movementInput;
        private Vector2 _lastMovementInput;
        
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            
            _characterView = StateMachine.GetExtension<CharacterView>();
            _playerInput = StateMachine.GetExtension<PlayerInput>();
            _transform = StateMachine.GetExtension<Transform>();
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

            if (_movementInput != Vector2.zero)
            {
                _characterView.PlayWalkAnimation(_movementInput);
                _lastMovementInput = _movementInput; 
            }
            else
            {
                StateMachine.ChangeNextState(new IdleCombatState());
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Move(_movementInput);
        }

        public override void OnExit()
        {
            base.OnExit();
            _characterView.PlayIdleAnimation(_lastMovementInput);
        }
        
        private void Move(Vector2 moveInput)
        {
            Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
            _transform.Translate(movement * (Time.fixedDeltaTime * _speed));
        }
    }
}