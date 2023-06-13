using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.FiniteStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        private State _nextState;
        private State _mainState;
        private Dictionary<Type, Component> _extensions = new Dictionary<Type, Component>();

        public T GetExtension<T>() where T : Component
        {
            if (_extensions.ContainsKey(typeof(T)))
            {
                return (T) _extensions[typeof(T)];
            }

            return null;
        }

        public void SetExtension<T>(T component) where T : Component
        {
            _extensions[typeof(T)] = component;
        }

        public void Initialize(State mainState)
        {
            _mainState = mainState;
            ChangeState(_mainState);
        }
        
        public void Initialize(State mainState, Dictionary<Type, Component> extensions)
        {
            _mainState = mainState;
            _extensions = extensions; 
            ChangeState(_mainState);
        }

        private void Update()
        {
            if (_nextState != null)
            {
                ChangeState(_nextState);
            }
            
            if (CurrentState != null)
            {
                CurrentState.OnUpdate();
            }
        }
        
        private void FixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }
        
        private void LateUpdate()
        {
            CurrentState.OnLateUpdate();
        }
        
        private void ChangeState(State state)
        {
            _nextState = null;
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }

            Debug.Log($"Changing state from {CurrentState} to {state}");
            
            CurrentState = state;
            CurrentState.OnEnter(this);
        }
        
        public void ChangeNextState(State state)
        {
            if (state != null)
                _nextState = state;
        }
        
        public void ChangeNextStateToMain()
        {
            ChangeNextState(_mainState);
        }
    }
}