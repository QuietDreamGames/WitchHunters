using Features.BTrees.Core;
using Features.BTrees.Interfaces;
using Features.Enemies.Extensions;
using Features.Enemies.Navigation;
using Features.Enemies.Steering;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public class OnCollisionLeaf : LeafNode
    {
        [Space] 
        [SerializeField] private Type type;
        
        [Space]
        [SerializeField] private bool isContinuous;
 
        private CollisionFacade _collisionFacade;

        private bool _isHandled;
        
        public override void Construct(IBTreeMachine stateMachine)
        {
            base.Construct(stateMachine);
            
            _collisionFacade = stateMachine.GetExtension<CollisionFacade>();
        }
                                    
        protected override void OnEnter()
        {
            _isHandled = false;
            switch (type)
            {
                case Type.Enter:
                    _collisionFacade.OnCollision2DEnter += Handle;
                    break;
                case Type.Stay:
                    _collisionFacade.OnCollision2DStay += Handle;
                    break;
                case Type.Exit:
                    _collisionFacade.OnCollision2DExit += Handle;
                    break;
            }
        }

        protected override void OnExit()
        {
            switch (type)
            {
                case Type.Enter:
                    _collisionFacade.OnCollision2DEnter -= Handle;
                    break;
                case Type.Stay:
                    _collisionFacade.OnCollision2DStay -= Handle;
                    break;
                case Type.Exit:
                    _collisionFacade.OnCollision2DExit -= Handle;
                    break;
            }
        }
                            
        protected override Status OnUpdate(float deltaTime)
        {
            return _isHandled 
                ? Status.Success 
                : isContinuous 
                    ? Status.Running 
                    : Status.Failure;
        }
        
        
        private void Handle(Collision2D other)
        {
            _isHandled = true;
        }
        
        private enum Type : byte
        {
            Enter = 0,
            Stay,
            Exit
        }
    }
}
