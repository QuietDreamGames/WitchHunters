using Features.FiniteStateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class MeleeBaseState : State
    {
        protected CharacterView CharacterView;
        protected PlayerInput PlayerInput;
        protected bool ShouldCombo;
        protected int attackIndex;
        
        
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            CharacterView = stateMachine.GetExtension<CharacterView>();
            PlayerInput = stateMachine.GetExtension<PlayerInput>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            ShouldCombo = PlayerInput.actions["Attack"].IsPressed();
        }
    }
}