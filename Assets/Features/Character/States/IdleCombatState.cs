using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class IdleCombatState : State
    {
        private readonly PlayerInput _playerInput;
        
        public IdleCombatState(IMachine stateMachine) : base(stateMachine)
        {
            _playerInput = stateMachine.GetExtension<PlayerInput>();
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["Attack"].IsPressed())
            {
                stateMachine.ChangeState("MeleeEntryState");
            }

            if (_playerInput.actions["Move"].IsPressed())
            {
                stateMachine.ChangeState("MoveState");
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