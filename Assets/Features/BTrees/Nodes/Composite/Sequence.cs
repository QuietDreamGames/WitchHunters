using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.BTrees.Nodes.Composite
{
    public class Sequence : CompositeNode
    {
        private int _currentIndex;

        protected override void OnEnter()
        {
            _currentIndex = 0;
        }

        protected override void OnExit()
        {
        }

        protected override Status OnUpdate(float deltaTime)
        {
            for (var i = _currentIndex; i < children.Length; i++)
            {
                _currentIndex = i;
                var child = children[_currentIndex];

                switch (child.UpdateCustom(deltaTime))
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        return Status.Failure;
                    case Status.Success:
                        continue;
                }
            }

            return Status.Success;
        }
    }
}
