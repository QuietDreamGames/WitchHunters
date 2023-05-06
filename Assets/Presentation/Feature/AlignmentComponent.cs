using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Presentation.Feature
{
    [BurstCompile]
    public struct AlignmentComponent : IComponentData
    {
        public bool Enabled;
        public float3 Value;
        
        public float Weight;
    }
}