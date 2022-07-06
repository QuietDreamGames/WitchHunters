using Features.Character.Components;
using Features.StateMachine.Components.Nodes.Composite;
using Features.StateMachine.Components.Nodes.Decorator;
using Features.StateMachine.Components.Nodes.Leaf;
using Features.StateMachine.Services.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using LogType = Features.StateMachine.Components.Nodes.Leaf.LogType;

namespace Features.StateMachine.Services
{
    public class EnemyBehaviourTestConvert : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var movement = new Movement
            {
                Direction = float3.zero,
                Enable = false
            };
            dstManager.AddComponentData(entity, movement);

            var speed = new Speed
            {
                Value = 1,
            };
            dstManager.AddComponentData(entity, speed);

            var mainNode = new TreeNode
            {
                Description = "BT root",
                Node = new Selector(3),
                Children = new []
                {
                    new TreeNode
                    {
                        Description = "failer test for selector",
                        Node = new Failer(),
                        Children = new []
                        {
                            new TreeNode
                            {
                                Description = "move up",
                                Node = new MoveDirection(new float3(0, 1, 0), 4),
                                Children = default
                            }, 
                        },
                    },
                    new TreeNode
                    {
                        Description = "succeeder test for selector",
                        Node = new Succeeder(),
                        Children = new []
                        {
                            new TreeNode
                            {
                                Description = "fck go back!",
                                Node = new Sequence(1),
                                Children = new []
                                {
                                    new TreeNode
                                    {
                                        Description = "go down",
                                        Node = new MoveDirection(new float3(0, -1, 0), 2),
                                        Children = default
                                    }
                                }
                            }, 
                        },
                    },
                    new TreeNode
                    {
                        Description = "horizontal movement",
                        Node = new Sequence(2),
                        Children = new []
                        {
                            new TreeNode
                            {
                                Description = "and left",
                                Node = new MoveDirection(new float3(-1, 0, 0), 5),
                                Children = default
                            },
                            new TreeNode
                            {
                                Description = "and right",
                                Node = new MoveDirection(new float3(1, 0, 0), 5),
                                Children = default
                            },
                        } 
                    }
                },
            };

            var loopNode = new TreeNode
            {
                Description = "repeat forever",
                Node = new Repeater(RepeaterType.Forever),
                Children = new[] { mainNode }
            };

            var logBranch = new TreeNode
            {
                Description = "reapeat 2 forever",
                Node = new Repeater(RepeaterType.Success),
                Children = new[]
                {
                    new TreeNode
                    { 
                        Description  = "wait loop seq",
                        Node = new Sequence(2),
                        Children = new []
                        {
                            new TreeNode
                            {
                                Description = "Wait for 3 sec", 
                                Node = new Wait(3),
                            },
                            new TreeNode
                            {
                                Description = "log some shit message ",
                                Node = new Log("shit message", LogType.Simple),
                            }
                        }
                    },
                }
            };

            var parallelNode = new TreeNode
            {
                Description = "parallel",
                Node = new Parallel(2, ParallelType.Selector),
                Children = new[]
                {
                    loopNode,
                    logBranch
                }
            };

            TreeNodeUtils.ConvertToEntity(in entity, ref dstManager, parallelNode);
        }
    }
}