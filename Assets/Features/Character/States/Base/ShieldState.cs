using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States.Base
{
    public class ShieldState : State
    {
        public ShieldState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            
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