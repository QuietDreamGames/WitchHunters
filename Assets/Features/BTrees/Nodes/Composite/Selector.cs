using System;
using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using UnityEngine;

namespace Features.BTrees.Nodes.Composite
{
    public class Selector : CompositeNode
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
                    case Status.Success:
                        return Status.Success;
                    case Status.Failure:
                        continue;
                }
            }

            return Status.Failure;
        }
    }
}