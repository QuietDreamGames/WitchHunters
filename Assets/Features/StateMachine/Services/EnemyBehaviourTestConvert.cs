using Features.StateMachine.Components.Nodes.Composite;
using Features.StateMachine.Components.Nodes.Decorator;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Services.Core;
using Unity.Entities;
using UnityEngine;
using LogType = Features.StateMachine.Components.Nodes.Leaf.LogType;

namespace Features.StateMachine.Services
{
    public class EnemyBehaviourTestConvert : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
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
                                Node = new GetClosestPlayer(3.75f) 
                            },
                        }
                    },
                    // some idling behaviour
                }
            };

            var genericMoveState = new TreeNode
            {
                Description = "move pattern",
                Node = new Parallel(2, ParallelType.Selector),
                Children = new[]
                {
                    new TreeNode
                    { 
                        Description  = "repeatedly get target for chasing",
                        Node = new Repeater(RepeaterType.Failed),
                        Children = new []
                        {
                            new TreeNode 
                            { 
                                Description = "get target in distance", 
                                Node = new GetClosestPlayer(4) 
                            },
                        }
                    },
                    new TreeNode
                    {
                        Description = "move to target",
                        Node = new MoveToTarget(.25f, 2)
                    }
                }
            };

            var genericAttackState = new TreeNode
            {
                Description = "attack pattern",
                Node = new Parallel(2, ParallelType.Sequence),
                Children = new[]
                {
                    new TreeNode 
                    { 
                        Description = "get target in distance", 
                        Node = new GetClosestPlayer(1) 
                    },
                    new TreeNode
                    {
                        Description = "attack debug test",
                        Node = new Sequence(2),
                        Children = new[]
                        {
                            new TreeNode
                            {
                                Description = "wait for debug",
                                Node = new Wait(1),
                            },
                            new TreeNode
                            {
                                Description = "attack debug",
                                Node = new Log("attack", LogType.Simple)
                            }
                        }
                    }
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

            TreeNodeUtils.ConvertToEntity(in entity, ref dstManager, genericEnemyBehaviour);
        }
    }
}