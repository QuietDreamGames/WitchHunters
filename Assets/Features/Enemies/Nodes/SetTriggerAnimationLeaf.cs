using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class SetTriggerAnimationLeaf : LeafNode
    {
        [SerializeField] private string trigger;

        private UnitView _unitView;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            _unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            _unitView.SetTrigger(trigger);
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
