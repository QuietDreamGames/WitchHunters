using Unity.Entities;
using Unity.Mathematics;

namespace Features.InputSystem.Components
{
    public struct LastMovementInput : IComponentData
    {
        public float3 Value;
    }
}