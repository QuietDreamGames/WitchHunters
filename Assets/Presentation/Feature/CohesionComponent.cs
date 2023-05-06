using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Presentation.Feature
{
    [BurstCompile]
    public struct CohesionComponent : IComponentData
    {
        public bool Enabled;
        public float3 Value;
        
        public float Weight;
    }
}