using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class MoveState : State
    {
        private CharacterView _characterView;
        private PlayerInput _playerInput;
        private Transform _transform;
        private ModifiersController _modifiersController;
        
        private float _speed = 5f;
        private Vector2 _movementInput;
        private Vector2 _lastMovementInput;
        
        public MoveState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void OnEnter()
        {
            _characterView = stateMachine.GetExtension<CharacterView>();
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _transform = stateMachine.GetExtension<Transform>();
            _modifiersController = stateMachine.GetExtension<ModifiersController>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["Attack"].IsPressed())
            {
                stateMachine.ChangeState("MeleeEntryState");
                return;
            }
            
            if (_playerInput.actions["Ultimate"].IsPressed())
            {
                var currentCooldownInfo = _modifiersController.GetModifierInfo(ModifierType.UltimateCurrentCooldown);
            
                if (currentCooldownInfo == null)
                {
                    stateMachine.ChangeState("UltimateSkillState");
                    return;
                }
            }
            
            _movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

            if (_movementInput != Vector2.zero)
            {
                _characterView.PlayWalkAnimation(_movementInput);
                _lastMovementInput = _movementInput; 
            }
            else
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            Move(_movementInput, deltaTime);
        }

        public override void OnLateUpdate(float deltaTime)
        {
            
        }

        public override void OnExit()
        {
            _characterView.PlayIdleAnimation(_lastMovementInput);
        }
        
        private void Move(Vector2 moveInput, float fixedDeltaTime)
        {
            Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
            _transform.Translate(movement * (fixedDeltaTime * _speed));
        }
    }
}