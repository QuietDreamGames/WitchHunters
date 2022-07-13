using Features.BehaviourTree.Components.Nodes.Composite;
using Features.BehaviourTree.Components.Nodes.Decorator;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Services.Core;
using Unity.Entities;
using UnityEngine;
using LogType = Features.BehaviourTree.Components.Nodes.Leaf.LogType;

namespace Features.BehaviourTree.Services
{
    public class EnemyBehaviourTestConvert : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            const float awareDistance = 4f;
            const float attackDistance = .2f;

            var idleState = new TreeNode
            {
                Description = "idle start",
                Node = new Log("idle started", LogType.Simple),
            };
            
            var moveState = new TreeNode
            {
                Description = "move to target pattern",
                Node = new Parallel(2, ParallelType.Selector),
                Children = new[]
                {
                    new TreeNode
                    {
                        Description = "repeatedly try to chase target",
                        Node = new Repeater(RepeaterType.Failed),
                        Children = new []
                        {
                            new TreeNode 
                            { 
                                Description = "get target in distance", 
                                Node = new GetClosestPlayer(awareDistance) 
                            }, 
                        }
                    },
                    new TreeNode
                    {
                        Description = "move to target",
                        Node = new MoveToTarget(attackDistance, 2)
                    } 
                }
            };
            
            var attackState = new TreeNode
            {
                Description = "attack pattern",
                Node = new Sequence(2),
                Children = new[]
                {
                    new TreeNode
                    {
                        Description = "attack animation pattern",
                        Node = new Parallel(2, ParallelType.Simple),
                        Children = new[]
                        {
                            new TreeNode
                            {
                                Description = "attack animation",
                                Node = new PlayAnimation("Attack1"),
                            },
                            new TreeNode
                            {
                                Description = "wait for damage deal pattern",
                                Node = new Sequence(2),
                                Children = new[]
                                {
                                    new TreeNode
                                    {
                                        Description = "wait for damage deal",
                                        Node = new Wait(.2f),
                                    },
                                    new TreeNode
                                    {
                                        Description = "damage deal debug",
                                        Node = new Log("damage deal", LogType.Simple),
                                    }
                                },
                            }
                        }
                    },
                    new TreeNode
                    {
                        Description = "wait after attack",
                        Node = new Wait(.25f),
                    },
                }
            };
            var genericEnemyBehaviour =
                GetState(idleState, moveState, attackState, awareDistance, attackDistance);

            TreeNodeUtils.ConvertToEntity(in entity, ref dstManager, genericEnemyBehaviour);
        }

        private TreeNode GetState(TreeNode idleState,
            TreeNode moveState,
            TreeNode attackState,
            float awareDistance,
            float attackDistance)
        {
            const float error = 0.25f;
            
            var genericIdleState = new TreeNode
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
                                Node = new GetClosestPlayer(awareDistance - error) 
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

            var genericMoveState = new TreeNode
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
                }
            };

            var genericAttackState = new TreeNode
            { 
                Description = "attack pattern",
                Node = new Sequence(2),
                Children = new []
                {
                    new TreeNode 
                    { 
                        Description = "get target in distance", 
                        Node = new GetClosestPlayer(attackDistance) 
                    },
                            
                    attackState
                }
            };

            var genericEnemyBehaviour = new TreeNode
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
                            genericAttackState,
                            genericMoveState,
                            genericIdleState
                        },
                    },
                },
            };

            return genericEnemyBehaviour;
        }
    }
}