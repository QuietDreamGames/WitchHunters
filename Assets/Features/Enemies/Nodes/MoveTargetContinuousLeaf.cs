using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.Enemies.Steering;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class MoveTargetContinuousLeaf : LeafNode, IFixedUpdateHandler
    {
        [Header("Dependencies")] 
        [SerializeField] private InTargetBoundsLeaf targetBounds;
 
        private Rigidbody2D _rigidbody2D;
        private UnitConfig _unitConfig;
        private ContextSteering _contextSteering;
        private TargetCollection _targetCollection;

        private Transform _currentTarget;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            _rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            _unitConfig = stateMachine.GetExtension<UnitConfig>();
            _contextSteering = stateMachine.GetExtension<ContextSteering>();
            _targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
                                    
        protected override void OnEnter()
        {
            
        }
                            
        protected override void OnExit()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            _currentTarget = _targetCollection.GetClosestTarget();
            if (_currentTarget == null)
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
            
            var origin = _rigidbody2D.position;
            Vector2 target = targetBounds == null 
                ? _currentTarget.position 
                : targetBounds.GetClosestPoint(origin);
            
            var direction = target - origin;
            direction.Normalize();
            
            if (!_contextSteering.IsDangerDirection(direction))
            {
                _contextSteering.SetInterest(direction);
            }

            var steering = _contextSteering.GetSteering();
            
            var velocity = steering * (_unitConfig.BaseSpeed * deltaTime);
            _rigidbody2D.velocity = velocity;
        }
    }
}
