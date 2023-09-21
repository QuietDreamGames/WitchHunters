using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class FaceClosestTargetLeaf : LeafNode
    {
        private new Rigidbody2D rigidbody2D;
        private UnitView unitView;
        private TargetCollection targetCollection;

        private Transform currentTarget;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            rigidbody2D = stateMachine.GetExtension<Rigidbody2D>();
            unitView = stateMachine.GetExtension<UnitView>();
            targetCollection = stateMachine.GetExtension<TargetCollection>();
        }
                                    
        protected override void OnEnter()
        {
            var origin = rigidbody2D.transform.position;
            currentTarget = targetCollection.GetClosestTarget();
            
            var direction = currentTarget.position - origin;
            unitView.SetFacingDirection(direction.x);
            unitView.SetMovementValueParams(direction.x, direction.y, 0);
        }
                            
        protected override void OnExit()
        {
            
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            return Status.Success;
        }
    }
}
