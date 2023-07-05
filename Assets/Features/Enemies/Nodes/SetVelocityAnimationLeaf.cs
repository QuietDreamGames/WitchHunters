using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetVelocityAnimationLeaf : LeafNode
    {
        private new Rigidbody2D rigidbody2D;
        private UnitView unitView;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            
        }
                            
        protected override void OnExit()
        {
            unitView.SetVelocity(Vector2.zero);
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            var velocity = rigidbody2D.velocity;
            unitView.SetVelocity(velocity);

            return Status.Running;
        }
    }
}
