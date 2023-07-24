using Features.BTrees.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.BTrees.Nodes.Decorator
{
    public class Repeat : DecoratorNode
    {
        [SerializeField] private bool restartOnSuccess = true;
        [SerializeField] private bool restartOnFailure = false;
        
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            switch (child.UpdateCustom(deltaTime))
            {
                case Status.Running:
                    break;
                case Status.Failure:
                    return restartOnFailure 
                        ? Status.Running 
                        : Status.Failure;
                case Status.Success:
                    return restartOnSuccess 
                        ? Status.Running 
                        : Status.Success;
            }

            return Status.Running;
        }
    }
}