using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Presentation.Feature
{
    [BurstCompile]
    public struct MoveComponent : IComponentData
    {
        public float2 Center;
        public float2 Size;

        public float3 ClampMin;
        public float3 ClampMax;
        
        public float2 WaitRange;
        
        public float Speed;
        
    }
}
