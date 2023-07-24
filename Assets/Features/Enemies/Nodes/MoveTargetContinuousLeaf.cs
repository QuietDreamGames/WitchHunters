using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.TimeSystems.Interfaces.Handlers;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class MoveTargetContinuousLeaf : LeafNode, IFixedUpdateHandler
    {
        [Header("Dependencies")] 
        [SerializeField] private InTargetBoundsLeaf targetBounds;
 
        private new Rigidbody2D rigidbody2D;
        private UnitConfig unitConfig;
        private TargetCollection targetCollection;

        private Transform currentTarget;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            unitConfig = stateMachine.GetExtension<UnitConfig>();
            targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
                                    
        protected override void OnEnter()
        {
            
        }
                            
        protected override void OnExit()
        {
            rigidbody2D.velocity = Vector2.zero;
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            currentTarget = targetCollection.GetClosestTarget();
            if (currentTarget == null)
            {
                return Status.Failure;
            }

            return Status.Running;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (currentStatus != Status.Running)
            {
                return;
            }
            
            float3 origin = rigidbody2D.transform.position;
            float3 target = targetBounds == null 
                ? currentTarget.position 
                : targetBounds.GetClosestPoint(origin);
            
            var direction = math.normalize(target - origin);
            var velocity = direction * unitConfig.BaseSpeed * deltaTime;
            
            rigidbody2D.velocity = velocity.xy;
        }
    }
}
