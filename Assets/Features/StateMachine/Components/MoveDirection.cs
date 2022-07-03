﻿using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Mathematics;

namespace Features.StateMachine.Components
{
    [BurstCompile]
    public struct MoveDirection: INodeComponent 
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