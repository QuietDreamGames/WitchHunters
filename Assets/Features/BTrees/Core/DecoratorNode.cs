using Features.BTrees.Interfaces;
using UnityEngine;

namespace Features.BTrees.Core
{
    public abstract class DecoratorNode : Node
    {
        [SerializeField] protected Node child;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            child.Construct(stateMachine);
        }
        
        public override void Abort()
        {
            base.Abort();
            if (child.CurrentStatus == Status.Running)
            {
                child.Abort();
            }
        }
    }
}