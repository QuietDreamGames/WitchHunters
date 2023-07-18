using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class IdleCombatState : State
    {
        private PlayerInput _playerInput;
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        private ShieldHealthController _shieldHealthController;
        
        public IdleCombatState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _modifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            _baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
            _shieldHealthController = stateMachine.GetExtension<ShieldHealthController>();
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
            
            if (_playerInput.actions["Secondary"].IsPressed())
            {
                var currentCooldownInfo = _modifiersContainer.GetValue(ModifierType.SecondaryCurrentCooldown,
                    0f);
                if (currentCooldownInfo <= 0) stateMachine.ChangeState("SecondarySkillState");
                return;
            }
            
            if (_playerInput.actions["Ultimate"].IsPressed())
            {
                var currentCooldownInfo = _modifiersContainer.GetValue(ModifierType.UltimateCurrentCooldown,
                    0f);
                if (currentCooldownInfo <= 0) stateMachine.ChangeState("UltimateSkillState");
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

            if (_playerInput.actions["Move"].IsPressed())
            {
                stateMachine.ChangeState("MoveState");
                return;
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