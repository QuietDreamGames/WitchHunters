using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class InTargetRadiusLeaf : AInTargetMeasureLeaf
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private float minRadius;
        [SerializeField] private float maxRadius;
        
        [Header("Debug")]
        [SerializeField] private Color debugColor = Color.green;
        
        private new RigidbodyAdapter rigidbody2D;
        private TargetCollection targetCollection;

        private Transform currentTarget;

        public override void Construct(IBTreeMachine stateMachine)
        {
            rigidbody2D = stateMachine.GetExtension<RigidbodyAdapter>();
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
            currentTarget = targetCollection.GetClosestTarget();
            if (currentTarget == null)
            {
                return Status.Failure;
            }
            
            var origin = rigidbody2D.Origin;
            Vector2 targetPosition = currentTarget.position;

            var distance = math.distance(origin, targetPosition);
            var isInRange = distance <= maxRadius && 
                            distance >= minRadius;
            
            return isInRange 
                ? Status.Success 
                : Status.Failure;
        }
        
        public override Vector3 GetClosestPoint(Vector3 origin)
        {
            var targetPosition = currentTarget.position;
            var distance = math.distance(origin, targetPosition);
            if (distance < maxRadius)
            {
                var direction = (origin - targetPosition).normalized;
                return targetPosition + direction * maxRadius;
            }
            
            if (distance > minRadius)
            {
                var direction = (targetPosition - origin).normalized;
                return targetPosition + direction * minRadius;
            }

            return origin;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = debugColor;

            var position = transform.position + offset;
            if (currentTarget != null)
            {
                position = currentTarget.position + offset;
            }
            
            Gizmos.DrawWireSphere(position, minRadius);
            Gizmos.DrawWireSphere(position, maxRadius);
        }
    }
}
