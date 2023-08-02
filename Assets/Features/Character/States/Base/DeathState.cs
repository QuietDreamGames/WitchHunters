using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States.Base
{
    public class DeathState : State
    {
        private CharacterView _characterView;
        
        public DeathState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _characterView = stateMachine.GetExtension<CharacterView>();
            _characterView.PlayDeathAnimation();
        }

        public override void OnExit()
        {
            _characterView.ResetTrigger();
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnFixedUpdate(float deltaTime)
        {
        }

        public override void OnLateUpdate(float deltaTime)
        {
        }
    }
}