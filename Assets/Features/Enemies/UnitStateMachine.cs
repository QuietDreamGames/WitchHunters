using System;
using System.Collections.Generic;
using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Enemies
{
    public class UnitStateMachine : MonoBehaviour, IBTreeMachine, IUpdateHandler
    {
        [Header("States")] 
        [SerializeField] private Node presentationState;
        [SerializeField] private Node combatState;
        [SerializeField] private Node hitState;
        [SerializeField] private Node deathState;
        
        [Header("Extensions")]
        [SerializeField] private Object[] extensions;
        
        private Node _currentState;

        private bool _isActive;

        public Action OnDeathExitHandle;

        #region States methods
        
        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            if (!_isActive)
            {
                if (_currentState != null)
                {
                    _currentState.Abort();
                }
            }
        }
        
        public void SetPresentationState()
        {
            SetState(presentationState);
            _currentState.OnExitHandler = SetCombatState;
        }

        public void SetCombatState()
        {
            SetState(combatState);
        }

        public void SetHitState()
        {
            SetState(hitState);
            _currentState.OnExitHandler = SetCombatState;
        }

        public void SetDeathState()
        {
            SetState(deathState);
            _currentState.OnExitHandler = OnDeathExitHandle;
        }

        private void SetState(Node state)
        {
            if (_currentState != null)
            {
                _currentState.Abort();
            }

            _currentState = state;
            _currentState.Construct(this);
            _currentState.OnEnterHandler = null;
            _currentState.OnExitHandler = null;
        }
        
        #endregion

        #region Extension methods

        public T GetExtension<T>() where T : Object
        {
            for (var i = 0; i < extensions.Length; i++)
            {
                if (extensions[i] is T)
                {
                    return (T) extensions[i];
                }
            }
            
            return null;
        }

        public IEnumerable<T> GetExtensions<T>() where T : Object
        {
            var list = new List<T>();

            for (var i = 0; i < extensions.Length; i++)
            {
                if (extensions[i] is T)
                {
                    list.Add((T) extensions[i]);
                }
            }

            return list;
        }
        
        #endregion
        

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }
            
            if (_currentState != null)
            {
                _currentState.UpdateCustom(deltaTime);
            }
        }
    }
}