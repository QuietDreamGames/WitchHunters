using Features.BTrees.Core;

namespace Features.BTrees.Nodes.Decorator
{
    public class Failure : DecoratorNode
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
            if (status == Status.Success)
            {
                return Status.Failure;
            }
            
            return status;
        }
    }
}