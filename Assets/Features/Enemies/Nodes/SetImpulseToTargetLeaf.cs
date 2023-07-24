using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetImpulseToTargetLeaf : LeafNode, IFixedUpdateHandler
    {
        [SerializeField] private float force;
        [SerializeField] private float duration;
        
        [SerializeField] private AnimationCurve curve;
        
        private new Rigidbody2D rigidbody2D;
        private UnitConfig unitConfig;
        private TargetCollection targetCollection;
        
        private Transform currentTarget;

        private float remainingDuration;
        private Vector3 currentDirection;
        private bool isRunning;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            unitConfig = stateMachine.GetExtension<UnitConfig>();
            targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
                                    
        protected override void OnEnter()
        {
            var targetPosition = targetCollection.FixatedTargetPosition;

            var origin = rigidbody2D.transform.position;
            currentDirection = (targetPosition - origin).normalized;
            
            remainingDuration = duration;
            isRunning = true;
        }
                            
        protected override void OnExit()
        {
            rigidbody2D.velocity = Vector2.zero;
            
            isRunning = false;
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            return isRunning 
                ? Status.Running 
                : Status.Success;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!isRunning)
            {
                return;
            }
            
            remainingDuration -= deltaTime;
            if (remainingDuration <= 0)
            {
                isRunning = false;
                return;
            }
            
            var t = 1 - remainingDuration / duration;
            var value = curve.Evaluate(t);
            
            var impulseForce = currentDirection * (force * unitConfig.SpeedMultiplier * value * deltaTime);
            rigidbody2D.velocity = impulseForce;
        }
    }
}
