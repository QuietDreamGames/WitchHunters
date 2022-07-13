using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct AttackOverlapBox : IComponentData
    {
        public bool Enable;
        public float2 Position1;
        public float2 Position2;
    }
}