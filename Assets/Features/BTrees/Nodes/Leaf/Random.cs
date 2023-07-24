using Features.BTrees.Core;
using UnityEngine;

namespace Features.BTrees.Nodes.Leaf
{
    public class Random : LeafNode
    {
        [SerializeField] private float odds = 0.5f;
        
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            var random = UnityEngine.Random.value;
            return random <= odds 
                ? Status.Success 
                : Status.Failure;
        }
    }
}