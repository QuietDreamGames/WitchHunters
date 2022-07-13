﻿using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct Wait : IComponentData, INode
    {
        public readonly float WaitTime;

        public float CurrentWaitTime;

        public Wait(float waitTime) : this()
        {
            WaitTime = waitTime;
        }
    }
}