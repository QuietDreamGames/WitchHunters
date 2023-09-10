using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.Enemies.Navigation;
using Features.Enemies.Steering;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class MoveTargetManageablyLeaf : LeafNode, IFixedUpdateHandler
    {
        [Header("Dependencies")] 
        [SerializeField] private AInTargetMeasureLeaf targetBounds;
 
        private RigidbodyAdapter _rigidbody2D;
        private UnitConfig _unitConfig;
        private UnitNavigation _unitNavigation;
        private ContextSteering _contextSteering;
        private TargetCollection _targetCollection;

        private Transform _currentTarget;
        private Vector2 _currentTargetPosition;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            _rigidbody2D = stateMachine.GetExtension<RigidbodyAdapter>();
            _unitConfig = stateMachine.GetExtension<UnitConfig>();
            _unitNavigation = stateMachine.GetExtension<UnitNavigation>();
            _contextSteering = stateMachine.GetExtension<ContextSteering>();
            _targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
                                    
        protected override void OnEnter()
        {
            _currentTarget = _targetCollection.GetClosestTarget();
            if (_currentTarget == null)
            {
                return;
            }
            
            _currentTargetPosition = targetBounds == null 
                ? _currentTarget.position 
                : targetBounds.GetClosestPoint(_rigidbody2D.Origin);            
            
            _rigidbody2D.Active = false;
        }
                            
        protected override void OnExit()
        {
            _rigidbody2D.Active = true; 
            _rigidbody2D.Velocity = Vector2.zero;
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            if (_currentTarget == null)
            {
                return Status.Failure;
            }

            SetupNavigation();
            
            return Status.Running;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (currentStatus != Status.Running)
            {
                return;
            }

            if (!_rigidbody2D.Active)
            {
                return;
            }
            
            var origin = _rigidbody2D.Origin;
            var target = _currentTargetPosition;
            _unitNavigation.SetTarget(target);
            var exist = _unitNavigation.TryGetDirection(out var direction);
            if (!exist)
            {
                direction = (target - origin).normalized;
            }
            
            if (!_contextSteering.IsDangerDirection(direction))
            {
                _contextSteering.SetInterest(direction);
            }
            var steering = _contextSteering.GetSteering();
            
            var velocity = steering * (_unitConfig.BaseSpeed * deltaTime);
            _rigidbody2D.Velocity = velocity;
        }
        
        private void SetupNavigation()
        {
            var active = _rigidbody2D.Active;
            if (active == _unitNavigation.IsActive)
            {
                return;
            }
            _unitNavigation.SetActive(active);
        }
    }
}
