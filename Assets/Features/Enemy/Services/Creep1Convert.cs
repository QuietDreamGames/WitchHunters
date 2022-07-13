﻿using Features.BehaviourTree.Components.Nodes.Composite;
using Features.BehaviourTree.Components.Nodes.Decorator;
using Features.BehaviourTree.Components.Nodes.Leaf;
using Features.BehaviourTree.Services;
using Features.BehaviourTree.Services.Core;
using Unity.Entities;
using UnityEngine;
using LogType = Features.BehaviourTree.Components.Nodes.Leaf.LogType;

namespace Features.Enemy.Services
{
    public class Creep1Convert : AGenericEnemyConvert
    {
        #region Serializable data

        [Header("Creep1 data")]
        
        [SerializeField] private string _attackAnimationName = "Attack1";
        [SerializeField] private float _moveSpeed = 2;

        #endregion
        
        #region AGenericEnemyBehaviour implementation

        protected override TreeNode GetIdleState()
        {
            return new TreeNode
            {
                Description = "idle start",
                Node = new Log("idle started", LogType.Simple),
            };
        }

        protected override TreeNode GetMoveState()
        {
            return new TreeNode
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
                                Node = new GetClosestPlayer(AwareDistance) 
                            }, 
                        }
                    },
                    new TreeNode
                    {
                        Description = "move to target",
                        Node = new MoveToTarget(AttackDistance, _moveSpeed)
                    },
                },
            };
        }

        protected override TreeNode GetAttackState()
        {
            return new TreeNode
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
                                Node = new PlayAnimation(_attackAnimationName),
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
                                    },
                                },
                            },
                        },
                    },
                    new TreeNode
                    {
                        Description = "wait after attack",
                        Node = new Wait(.25f),
                    },
                },
            };
        }

        protected override void AddIndividualComponentsData(in Entity entity, 
            ref EntityManager dstManager,
            GameObjectConversionSystem conversionSystem)
        {
            
        }

        #endregion
        
        
    }
}