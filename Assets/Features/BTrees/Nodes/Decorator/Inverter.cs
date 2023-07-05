using Features.BTrees.Core;

namespace Features.BTrees.Nodes.Decorator
{
    public class Inverter : DecoratorNode
    {
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
                    return Status.Running;
                case Status.Failure:
                    return Status.Success;
                case Status.Success:
                    return Status.Failure;
            }

            return Status.Failure;
        }
    }
}