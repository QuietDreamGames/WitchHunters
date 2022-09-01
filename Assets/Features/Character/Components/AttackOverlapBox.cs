using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct AttackOverlapBox : IComponentData
    {
        public bool Enable;

        public float2 CenterOffset;

        public float2 HorizontalOffset;
        public float2 VerticalOffset;

        public float2 HorizontalSize;
        public float2 VerticalSize;
    }
}