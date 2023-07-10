using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class IdleCombatState : State
    {
        private PlayerInput _playerInput;
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        
        public IdleCombatState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _modifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            _baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["Attack"].IsPressed())
            {
                stateMachine.ChangeState("MeleeEntryState");
                return;
            }

            if (_playerInput.actions["Move"].IsPressed())
            {
                stateMachine.ChangeState("MoveState");
                return;
            }
            
            if (_playerInput.actions["Ultimate"].IsPressed())
            {
                var currentCooldownInfo = _modifiersContainer.GetValue(ModifierType.UltimateCurrentCooldown,
                    0f);
                if (currentCooldownInfo > 0) return;
                stateMachine.ChangeState("UltimateSkillState");
            }
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            
        }

        public override void OnLateUpdate(float deltaTime)
        {
            
        }
    }
}