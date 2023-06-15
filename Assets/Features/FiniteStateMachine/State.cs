using Features.FiniteStateMachine.Interfaces;

namespace Features.FiniteStateMachine
{
    public abstract class State
    {
        protected IMachine stateMachine;
        
        public State(IMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate(float deltaTime);
        public abstract void OnFixedUpdate(float deltaTime);
        public abstract void OnLateUpdate(float deltaTime);
    }
}