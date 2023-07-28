using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States.Base
{
    public class DeathState : State
    {
        public DeathState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            var characterView = stateMachine.GetExtension<CharacterView>();
            characterView.PlayDeathAnimation();
        }

        public override void OnExit()
        {
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