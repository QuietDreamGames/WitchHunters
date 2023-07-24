using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class TargetInDistanceLeaf : LeafNode
    {
        [SerializeField] private float range;

        [Header("Debug")] 
        [SerializeField] private Color debugColor = Color.green;
        
        private new Rigidbody2D rigidbody2D;
        private TargetCollection targetCollection;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
        
        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }

        protected override Status OnUpdate(float deltaTime)
        {
            var target = targetCollection.GetClosestTarget();
            if (target == null)
            {
                return Status.Failure;
            }
            
            var origin = rigidbody2D.transform.position;
            var distance = math.distance(origin, target.position);
            var isInRange = distance <= range;

            return isInRange 
                ? Status.Success 
                : Status.Failure;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
