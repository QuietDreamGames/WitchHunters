using System;
using UnityEngine;

namespace Features.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        private State _nextState;

        private void Update()
        {
            if (_nextState != null)
            {
                SetState(_nextState);
                _nextState = null;
            }
            
            CurrentState.OnUpdate();
        }
        
        private void FixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }
        
        private void LateUpdate()
        {
            CurrentState.OnLateUpdate();
        }
        
        private void SetState(State state)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }
            
            CurrentState = state;
            CurrentState.OnEnter(this);
        }
        
        public void SetNextState(State state)
        {
            if (state != null)
                
                _nextState = state;
        }
    }
}