using Features.BTrees.Core;

namespace Features.BTrees.Nodes.Decorator
{
    public class Succeed : DecoratorNode
    {
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            var status = child.UpdateCustom(deltaTime);
            if (status == Status.Failure)
            {
                return Status.Success;
            }
            
            return status;
        }
    }
}