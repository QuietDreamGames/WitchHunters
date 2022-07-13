﻿using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct MoveDirection: IComponentData, INode 
    {
        public readonly float3 Direction;
        public readonly float Time;
        
        public float CurrentTime;
        
        public MoveDirection(float3 direction, float time) : this()
        {
            Direction = direction;
            Time = time;
        }
    }
}