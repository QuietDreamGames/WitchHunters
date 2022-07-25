using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct AttackOverlapBox : IComponentData
    {
        public bool Enable;
        public float Width;
        public float Height;
        public float2 OffsetXY;
    }
}