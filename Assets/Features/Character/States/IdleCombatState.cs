using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class IdleCombatState : State
    {
        private PlayerInput _playerInput;
        private ModifiersController _modifiersController;
        
        public IdleCombatState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _modifiersController = stateMachine.GetExtension<ModifiersController>();
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
                var currentCooldownInfo = _modifiersController.GetModifierInfo(ModifierType.UltimateCurrentCooldown);
            
                if (currentCooldownInfo == null)
                {
                    stateMachine.ChangeState("UltimateSkillState");
                    return;
                }
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