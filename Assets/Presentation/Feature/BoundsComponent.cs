using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Presentation.Feature
{
    [BurstCompile]
    public struct BoundsComponent : IComponentData
    {
        public float3 Center;
        public float2 Size;
        
        public float3 Value;
        public bool Enabled;
    }
}