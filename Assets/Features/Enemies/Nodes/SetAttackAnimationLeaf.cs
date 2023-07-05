using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetAttackAnimationLeaf : LeafNode
    {
        [SerializeField] private int attackID;
        
        private UnitView unitView;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            unitView.SetAttack(attackID);
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
