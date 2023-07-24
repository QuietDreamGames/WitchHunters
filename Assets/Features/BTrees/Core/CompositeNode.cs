using Features.BTrees.Interfaces;
using UnityEngine;

namespace Features.BTrees.Core
{
    public abstract class CompositeNode : Node
    {
        [SerializeField] protected Node[] children;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];
                child.Construct(stateMachine);
            }
        }
        
        public override void Abort()
        {
            base.Abort();
            AbortRunningChildren();
        }
        
        protected void AbortRunningChildren()
        {
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];
                if (child.CurrentStatus == Status.Running)
                {
                    child.Abort();
                }
            }
        }
    }
}