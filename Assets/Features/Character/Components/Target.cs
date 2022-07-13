using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct Target : IComponentData
    {
        public float3 Value;
    }
}