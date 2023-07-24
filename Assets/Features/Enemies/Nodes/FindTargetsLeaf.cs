using System;
using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class FindTargetsLeaf : LeafNode
    {
        [SerializeField] private float range;
        [SerializeField] private LayerMask layerMask;

        [Header("Debug")] 
        [SerializeField] private Color debugColor = Color.green;
        
        private readonly RaycastHit2D[] raycastHits = new RaycastHit2D[10];
        
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
            var count = Physics2D.CircleCastNonAlloc(
                transform.position,
                range,
                Vector2.zero,
                raycastHits,
                0,
                layerMask);
            
            targetCollection.Clear();
            var origin = rigidbody2D.transform.position;
            for (var i = 0; i < count; i++)
            {
                var raycastHit = raycastHits[i];
                var target = raycastHit.transform;
                var distance = math.distance(origin, target.position);
                targetCollection.AddTarget(target, distance);
            }

            return count != 0 
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
