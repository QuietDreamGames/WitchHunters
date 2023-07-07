using Features.BTrees.Core;
using UnityEngine;

namespace Features.BTrees.Nodes.Composite
{
    public class Prioritize : CompositeNode
    {
        private int previousChildIndex;
        
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];

                switch (child.UpdateCustom(deltaTime))
                {
                    case Status.Running:
                        var currentChildIndex = i;
                        if (currentChildIndex < previousChildIndex)
                        {
                            for (var j = currentChildIndex; j < previousChildIndex + 1; j++)
                            {
                                children[j].Abort();
                            }
                        }
                        previousChildIndex = i;
                        return Status.Running;
                    case Status.Success:
                        return Status.Success;
                    case Status.Failure:
                        child.Abort();
                        continue;
                }
            }

            return Status.Failure;
        }
    }
}