using System;
using System.Collections.Generic;
using Features.FiniteStateMachine.Interfaces;
using UnityEngine;

namespace Features.FiniteStateMachine
{
    public class StateMachine : IMachine
    {
        private Dictionary<string, State> states;
        private List<object> extensions;

        private State currentState;

        public StateMachine()
        {
            states = new Dictionary<string, State>();
            extensions = new List<object>();
            currentState = null;
        }

        public void AddState(string stateID, State state)
        {
            states.Add(stateID, state);
        }

        public void OnUpdate(float deltaTime)
        {
            if (currentState != null)
            {
                currentState.OnUpdate(deltaTime);
            }
        }
        
        public void OnFixedUpdate(float deltaTime)
        {
            if (currentState != null)
            {
                currentState.OnFixedUpdate(deltaTime);
            }
        }
        
        public void OnLateUpdate(float deltaTime)
        {
            if (currentState != null)
            {
                currentState.OnLateUpdate(deltaTime);
            }
        }

        public void ChangeState(string stateID)
        {
            if (currentState != null)
            {
                currentState.OnExit();
            }

            var containsState = states.TryGetValue(stateID, out currentState);
            if (containsState == false)
            {
                Debug.LogError($"State with ID {stateID} not found in FSM");
                return;
            }

            currentState.OnEnter();
            currentState.OnUpdate(0); 
        }

        public void AddExtension<T>(T extension) where T : class
        {
            if (extensions.Contains(extension))
            {
                Debug.LogError($"Extension {extension} already added to FSM");
                return;
            }
            
            extensions.Add(extension);
        }

        public T GetExtension<T>() where T : class
        {
            for (var i = 0; i < extensions.Count; i++)
            {
                var extension = extensions[i];
                if (extension is T)
                {
                    return extension as T;
                }
            }
            
            Debug.LogError($"Extension of type {typeof(T).FullName} not found in FSM");
            return null;
        }

        public IEnumerable<T> GetExtensions<T>() where T : class
        {
            var list = new List<T>();
            
            for (var i = 0; i < extensions.Count; i++)
            {
                var extension = extensions[i];
                if (extension is T)
                {
                    list.Add(extension as T);
                }
            }
            
            if (list.Count == 0)
            {
                Debug.LogError($"Extensions of type {typeof(T).FullName} not found in FSM");
                return null;
            }

            return list;
        }
    }
}