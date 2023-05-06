using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Presentation.Feature
{
    [BurstCompile]
    public struct FlockComponent : IComponentData
    {
        public float3 Velocity;
        
        public float MaxSpeed;
    }
}
