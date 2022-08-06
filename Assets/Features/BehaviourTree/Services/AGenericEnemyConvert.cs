using Features.Animator.Components;
using Features.BehaviourTree.Components.Nodes.Composite;
using Features.BehaviourTree.Components.Nodes.Decorator;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Services.Core;
using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.BehaviourTree.Services
{
    public abstract class AGenericEnemyConvert : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Generic Enemy Params")]
        
        [SerializeField] protected float AwareDistance = 5f;
        [SerializeField] protected float AttackDistance = 1f;

        #endregion

        #region Private fields

        private const float ERROR_DISTANCE = 0.25f;

        #endregion
        
        #region Virtual methods

        protected abstract TreeNode GetIdleState();
        protected abstract TreeNode GetMoveState();
        protected abstract TreeNode GetAttackState();

        protected abstract void AddIndividualComponentsData(in Entity entity,
            ref EntityManager dstManager,
            GameObjectConversionSystem conversionSystem);
        
        #endregion
        
        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            AddGeneralComponentsData(in entity, ref dstManager, conversionSystem);
            
            AddIndividualComponentsData(in entity, ref dstManager, conversionSystem);
            
            AddBehaviourTreeData(in entity, ref dstManager);
        }

        #endregion

        #region Gizmos methods

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, AwareDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, AttackDistance);
        }

        #endregion

        #region Private methods

        private void AddGeneralComponentsData(in Entity entity,
            ref EntityManager dstManager,
            GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Movement());
            dstManager.AddComponentData(entity, new Speed());
            
            dstManager.AddComponentData(entity, new Target());

            dstManager.AddComponentData(entity, new Attack());
            dstManager.AddComponentData(entity, new AttackOverlapBox());

            dstManager.AddComponentData(entity, new PlayNextAnimation());
        }

        private void AddBehaviourTreeData(in Entity entity, ref EntityManager dstManager)
        {
            var rootBehaviour = CombineBehaviour(GetIdleState(),
                GetMoveState(),
                GetAttackState(),
                AwareDistance,
                AttackDistance);

            TreeNodeUtils.ConvertToEntity(in entity, ref dstManager, rootBehaviour);
        }
        
        private TreeNode CombineBehaviour(TreeNode idleState,
            TreeNode moveState,
            TreeNode attackState,
            float awareDistance,
            float attackDistance)
        {
            var finalIdleState = CombineIdleState(idleState, awareDistance);
            var finalMoveState = CombineMoveState(moveState, awareDistance);
            var finalAttackState = CombineAttackState(attackState, attackDistance);
            
            return new TreeNode
            {
                Description = "generic behaviour repeater",
                Node = new Repeater(RepeaterType.Forever),
                Children = new []
                {
                    new TreeNode
                    {
                        Description = "generic behaviour states",
                        Node = new Selector(3),
                        Children = new []
                        {
                            finalAttackState,
                            finalMoveState,
                            finalIdleState
                        },
                    },
                },
            };
        }
        
        private TreeNode CombineIdleState(TreeNode idleState, 
            float awareDistance)
        {
            return new TreeNode
            {
                Description = "idle pattern",
                Node = new Parallel(2, ParallelType.Selector),
                Children = new[]
                {
                    new TreeNode
                    { 
                        Description  = "repeatedly get target for leave idle",
                        Node = new Repeater(RepeaterType.Success),
                        Children = new []
                        {
                            new TreeNode 
                            { 
                                Description = "get target in distance", 
                                Node = new GetClosestPlayer(awareDistance - ERROR_DISTANCE) 
                            },
                        }
                    },
                    new TreeNode
                    {
                        Description = "failer for correct paralleling",
                        Node = new Failer(),
                        Children = new []
                        {
                            idleState
                        }
                    }
                }
            };
        }

        private TreeNode CombineMoveState(TreeNode moveState, 
            float awareDistance)
        {
            return new TreeNode
            { 
                Description = "move pattern",
                Node = new Sequence(2),
                Children = new []
                {
                    new TreeNode 
                    { 
                        Description = "get target in distance", 
                        Node = new GetClosestPlayer(awareDistance) 
                    },
                            
                    moveState
                },
            };
        }

        private TreeNode CombineAttackState(TreeNode attackState, 
            float attackDistance)
        {
            return new TreeNode
            { 
                Description = "attack pattern",
                Node = new Sequence(2),
                Children = new []
                {
                    new TreeNode 
                    { 
                        Description = "get target in distance", 
                        Node = new GetClosestPlayer(attackDistance + ERROR_DISTANCE) 
                    },
                            
                    attackState
                },
            };
        }

        #endregion
    }
}