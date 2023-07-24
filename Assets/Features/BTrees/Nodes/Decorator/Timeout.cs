using Features.BTrees.Core;
using UnityEngine;

namespace Features.BTrees.Nodes.Decorator
{
    public class Timeout : DecoratorNode
    {
        [SerializeField] private float duration = 1;
        
        private float _waitTime;
        
        protected override void OnEnter()
        {
            _waitTime = 0;
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            if (_waitTime >= duration)
            {
                return Status.Failure;
            }
            
            _waitTime += deltaTime;
            return child.UpdateCustom(deltaTime);
        }
    }
}