using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class WaitEventLeaf : LeafNode
    {
        [SerializeField] private string eventName;
        
        private UnitView unitView;
        
        private bool isComplete;

        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            unitView = stateMachine.GetExtension<UnitView>();
        }
                                    
        protected override void OnEnter()
        {
            unitView.OnEvent += OnEvent;
            
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

        private void OnEvent(string eventName)
        {
            if (eventName != this.eventName)
            {
                return;
            }
            
            isComplete = true;
        }
    }
}
