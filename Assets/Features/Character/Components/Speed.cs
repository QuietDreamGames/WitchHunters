using Unity.Burst;
using Unity.Entities;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct Speed : IComponentData
    {
        public float Value;
    }
}