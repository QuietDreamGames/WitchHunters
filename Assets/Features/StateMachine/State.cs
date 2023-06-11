using UnityEngine;

namespace Features.StateMachine
{
    public abstract class State
    {
        protected float time { get; set; }
        protected float fixedTime { get; set; }
        protected float lateTime { get; set; }
        
        public StateMachine StateMachine;
        
        public virtual void OnEnter()
        {
            
        }
        
        public virtual void OnEnter(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        public virtual void OnUpdate()
        {
            time += Time.deltaTime;
        }
        
        public virtual void OnFixedUpdate()
        {
            fixedTime += Time.deltaTime;
        }
        
        public virtual void OnLateUpdate()
        {
            lateTime += Time.deltaTime;
        }
        
        public virtual void OnExit()
        {
            
        }

        #region Passthrough methods
        
        protected static void Destroy(Object obj)
        {
            Object.Destroy(obj);
        }
        
        protected T GetComponent<T>() where T : Component
        {
            return StateMachine.GetComponent<T>();
        }
        
        protected Component GetComponent(System.Type type)
        {
            return StateMachine.GetComponent(type);
        }

        protected Component GetComponent(string type)
        {
            return StateMachine.GetComponent(type);
        }
        
        #endregion
    }
}