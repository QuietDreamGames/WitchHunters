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
                        FinishChildren(i);
                        previousChildIndex = i;
                        return Status.Running;
                    case Status.Success:
                        FinishChildren(i);
                        return Status.Success;
                    case Status.Failure:
                        child.Abort();
                        continue;
                }
            }

            return Status.Failure;
        }

        private void FinishChildren(int currentChildIndex)
        {
            var nextChildIndex = currentChildIndex + 1;
            if (nextChildIndex <= previousChildIndex)
            {
                for (var j = nextChildIndex; j < previousChildIndex + 1; j++)
                {
                    children[j].Abort();
                }
            } 
        }
    }
}