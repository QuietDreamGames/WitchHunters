﻿using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class MoveState : State
    {
        private CharacterView _characterView;
        private PlayerInput _playerInput;
        private Rigidbody2D _rigidbody;
        private SkillsController _skillsController;
        private ShieldHealthController _shieldHealthController;
        
        private BaseModifiersContainer _baseModifiersContainer;
        private ModifiersContainer _modifiersContainer;
        
        private float _baseMoveSpeed;
        private float _animationSpeedMultiplier = 1f;
        
        private Vector2 _movementInput;
        private Vector2 _lastMovementInput;
        
        public MoveState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void OnEnter()
        {
            _characterView = stateMachine.GetExtension<CharacterView>();
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _rigidbody = stateMachine.GetExtension<Rigidbody2D>();
            _skillsController = stateMachine.GetExtension<SkillsController>();
            _shieldHealthController = stateMachine.GetExtension<ShieldHealthController>();
            
            _baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
            _modifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            
            _baseMoveSpeed = _baseModifiersContainer.GetBaseValue(ModifierType.MoveSpeed);
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["Attack"].IsPressed())
            {
                stateMachine.ChangeState("MeleeEntryState");
                return;
            }
            
            if (_playerInput.actions["Secondary"].IsPressed())
            {
                
                if (!_skillsController.Secondary.IsOnCooldown) stateMachine.ChangeState("SecondarySkillState");
                return;
            }
            
            if (_playerInput.actions["Ultimate"].IsPressed())
            {
                
                if (!_skillsController.Ultimate.IsOnCooldown) stateMachine.ChangeState("UltimateSkillState");
                return;
            }
            
            if (_playerInput.actions["Shield"].IsPressed())
            {
                _shieldHealthController.GetShieldHealth(out var shieldCurrentHealth, out var shieldMaxHealth);
                if (shieldCurrentHealth > 0)
                {
                    stateMachine.ChangeState("ShieldState");
                    return;
                }
            }
            
            _movementInput = _playerInput.actions["Move"].ReadValue<Vector2>();

            if (_movementInput != Vector2.zero)
            {
                _characterView.PlayWalkAnimation(_movementInput, _animationSpeedMultiplier);
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
            var speed = _modifiersContainer.GetValue(ModifierType.MoveSpeed, _baseMoveSpeed);
            _animationSpeedMultiplier = speed / _baseMoveSpeed;
            Vector2 movement = _rigidbody.position + moveInput * (speed * fixedDeltaTime);
            _rigidbody.MovePosition(movement);
        }
    }
}