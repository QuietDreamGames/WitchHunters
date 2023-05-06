using Unity.Entities;
using Unity.Mathematics;

namespace Presentation.Feature
{
    public struct SeparationComponent : IComponentData
    {
        public bool Enabled;
        public float3 Value;
        
        public float Weight;
    }
}