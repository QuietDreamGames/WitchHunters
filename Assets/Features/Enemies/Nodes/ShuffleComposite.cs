using Edgar.Legacy.Utils;
using Features.BTrees.Core;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class ShuffleComposite : CompositeNode
    {
        private int _currentIndex;

        protected override void OnEnter()
        {
            Shuffle();
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
        
        private void Shuffle()
        {
            for (var i = children.Length - 1; i > 0; i--)
            {
                var rnd = Random.Range(0, i);
                (children[i], children[rnd]) = (children[rnd], children[i]);
            }
        }
    }
}
