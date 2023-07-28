using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetVelocityAnimationLeaf : LeafNode
    {
        private Rigidbody2D _rigidbody2D;
        private UnitView _unitView;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            _rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            _unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            
        }
                            
        protected override void OnExit()
        {
            _unitView.SetVelocity(Vector2.zero);
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            var velocity = _rigidbody2D.velocity;
            _unitView.SetVelocity(velocity);

            return Status.Running;
        }
    }
}
