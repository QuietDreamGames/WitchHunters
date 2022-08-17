using Unity.Burst;
using Unity.Entities;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct DeltaTime : IComponentData
    {
        public float Value;
    }
}