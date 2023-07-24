using Features.BTrees.Core;
using UnityEngine;

namespace Features.BTrees.Nodes.Leaf
{
    public class Log : LeafNode
    {
        [SerializeField] private string message;
        
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            Debug.Log(message);
            return Status.Success;
        }
    }
}