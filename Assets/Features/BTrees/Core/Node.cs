using System;
using Features.BTrees.Interfaces;
using Unity.Collections;
using UnityEngine;

namespace Features.BTrees.Core
{
    public abstract class Node : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected Status currentStatus = Status.Success;
        [SerializeField, ReadOnly] protected bool started = false;
        
        public Status CurrentStatus => currentStatus;

        public Action OnEnterHandler;
        public Action OnExitHandler;

        public Status UpdateCustom(float deltaTime)
        {
            if (!started)
            {
                OnEnter();
                OnEnterHandler?.Invoke();
                started = true;
            }

            currentStatus = OnUpdate(deltaTime);

            if (currentStatus != Status.Running)
            {
                OnExit();
                OnExitHandler?.Invoke();
                started = false;
            }

            return currentStatus;
        }

        public virtual void Construct(IBTreeMachine stateMachine)
        {
            
        }
        
        public virtual void Abort()
        {
            started = false;
            currentStatus = Status.Failure;
            OnExit();
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract Status OnUpdate(float deltaTime);
    }
}