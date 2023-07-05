using System;
using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class InTargetBoundsLeaf : LeafNode
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 size;
        
        [Header("Debug")]
        [SerializeField] private Color debugColor = Color.green;
        
        private new Rigidbody2D rigidbody2D;
        private TargetCollection targetCollection;

        private Transform currentTarget;
        private Vector3 currentOffset;
        
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
            currentTarget = targetCollection.GetClosestTarget();
            if (currentTarget == null)
            {
                return Status.Failure;
            }
            
            var origin = rigidbody2D.transform.position;
            var targetPosition = currentTarget.position;

            currentOffset = GetCurrentOffset(origin, targetPosition, offset);

            var targetBounds = new Bounds(targetPosition + currentOffset, size);
            var isInRange = targetBounds.Contains(origin);
            
            return isInRange 
                ? Status.Success 
                : Status.Failure;
        }
        
        private Vector3 GetCurrentOffset(Vector3 origin, Vector3 target, Vector3 offset)
        {
            var direction = target - origin;
            direction = -direction;
            var sign = math.sign(direction.x);
            offset.x = math.abs(offset.x) * sign;

            return offset;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = debugColor;
            
            if (currentTarget == null)
            {
                Gizmos.DrawWireCube(transform.position + offset, size);
            }
            else
            {
                Gizmos.DrawWireCube(currentTarget.position + currentOffset, size);
            }
        }
    }
}
