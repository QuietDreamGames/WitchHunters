﻿using Features.Character.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Character.Systems
{
    public partial class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new MoveJob { DeltaTime = Time.DeltaTime }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(ref Translation translation, in Movement movement, in Speed speed)
        {
            translation.Value += new float3(movement.Value * speed.Value * DeltaTime, 0);
        }
    }
}