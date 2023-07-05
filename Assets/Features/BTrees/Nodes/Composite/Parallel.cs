using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using UnityEngine;

namespace Features.BTrees.Nodes.Composite
{
    public class Parallel : CompositeNode
    {
        [SerializeField] private bool byFirst = false; 
    
        private Status[] _childrenExecute;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            _childrenExecute = new Status[children.Length];
        }

        protected override void OnEnter()
        {
            for (var i = 0; i < children.Length; i++)
            {
                _childrenExecute[i] = Status.Running;
            }
        }

        protected override void OnExit()
        {
        }

        protected override Status OnUpdate(float deltaTime)
        {
            var stillRunning = false;
            for (var i = 0; i < _childrenExecute.Length; ++i)
            {
                if (_childrenExecute[i] == Status.Running)
                {
                    var status = children[i].UpdateCustom(deltaTime);
                    if (status == Status.Failure)
                    {
                        AbortRunningChildren();
                        return Status.Failure;
                    }

                    if (byFirst)
                    {
                        if (status == Status.Success)
                        {
                            AbortRunningChildren();
                            return Status.Success;
                        }
                    }

                    if (status == Status.Running)
                    {
                        stillRunning = true;
                    }

                    _childrenExecute[i] = status;
                }
            }

            return stillRunning 
                ? Status.Running 
                : Status.Success;
        }
    }
}