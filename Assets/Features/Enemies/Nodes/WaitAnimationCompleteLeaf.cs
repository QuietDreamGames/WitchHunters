using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class WaitAnimationCompleteLeaf : LeafNode
    {
        private UnitView unitView;
        
        private bool isComplete;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            unitView.OnCompleteAnimation += OnCompleteAnimation;
            
            isComplete = false;
        }

        protected override void OnExit()
        {
            
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            return !isComplete 
                ? Status.Running 
                : Status.Success;
        }

        private void OnCompleteAnimation()
        {
            isComplete = true;
        }
    }
}
