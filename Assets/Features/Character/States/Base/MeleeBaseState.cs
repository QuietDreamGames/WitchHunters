using System.Collections;
using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Network;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class MeleeBaseState : State
    {
        protected CharacterView CharacterView;
        protected PlayerInput PlayerInput;
        protected NetworkInput NetworkInput;
        protected bool ShouldCombo;
        protected int attackIndex;
        protected float attackSpeed;
        
        // private float _attackPressedTimer = 0;
        
        private Vector2 _lastMovementDirection;

        public MeleeBaseState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            CharacterView = stateMachine.GetExtension<CharacterView>();
            PlayerInput = stateMachine.GetExtension<PlayerInput>();
            NetworkInput = stateMachine.GetExtension<NetworkInput>();
            var modifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            var baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
            attackSpeed = modifiersContainer.GetValue(ModifierType.AttackSpeed,
                baseModifiersContainer.GetBaseValue(ModifierType.AttackSpeed));
            _lastMovementDirection = Vector2.zero;
            // _collidersDamaged = new List<Collider2D>();
            // HitCollider = stateMachine.GetExtension<Collider2D>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            // _attackPressedTimer -= Time.deltaTime;
            //
            // if (CharacterView.IsAttackColliderActive())
            // {
            //     Attack();
            // }
            //
            // if (PlayerInput.actions["Attack"].IsPressed())
            // {
            //     _attackPressedTimer = 1f;
            // }
            //
            // ShouldCombo = _attackPressedTimer > 0;
            
            // if (CharacterView.IsAttackColliderActive())
            // {
            //     Attack();
            // }
            
            var inputData = NetworkInput.InputData;

            ShouldCombo = inputData.attack;
            
            var movementInput = inputData.move;

            if (movementInput != Vector2.zero)
            {
                _lastMovementDirection = movementInput;
            }
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            
        }

        public override void OnLateUpdate(float deltaTime)
        {
            
        }
        
        public override void OnExit()
        {
            if (_lastMovementDirection != Vector2.zero)
                CharacterView.SetLastMovementDirection(_lastMovementDirection);
        }
    }
}