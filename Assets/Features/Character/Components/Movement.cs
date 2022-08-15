using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct Movement : IComponentData
    {
        public float3 Direction;
        public bool Enable;
        public bool Trigger;
    }
}
