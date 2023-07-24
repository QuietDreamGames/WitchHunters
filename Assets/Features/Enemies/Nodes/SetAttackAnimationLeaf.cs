using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetAttackAnimationLeaf : LeafNode
    {
        [SerializeField] private int attackID;

        [SerializeField] private bool fixateTarget = true;
        
        private UnitView unitView;
        private TargetCollection targetCollection;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            unitView = stateMachine.GetExtension<UnitView>();
            if (fixateTarget)
            {
                targetCollection = stateMachine.GetExtension<TargetCollection>();
            }
        }
                                    
        protected override void OnEnter()
        {
            unitView.SetAttack(attackID);
            if (fixateTarget)
            {
                var target = targetCollection.GetClosestTarget();
                if (target != null)
                {
                    targetCollection.FixatedTargetPosition = target.position;
                }
            }
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
