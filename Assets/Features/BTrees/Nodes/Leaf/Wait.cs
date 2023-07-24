using Features.BTrees.Core;
using UnityEngine;

namespace Features.BTrees.Nodes.Leaf
{
    public class Wait : LeafNode
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
                return Status.Success;
            }
            
            _waitTime += deltaTime;
            return Status.Running;
        }
    }
}