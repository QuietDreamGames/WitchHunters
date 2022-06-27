using Unity.Entities;
using Unity.Mathematics;

namespace Features.Collision.Components
{
    public struct Collider : IComponentData
    {
        public float3 Size;
    }
}
