using System;
using Unity.Burst;
using Unity.Mathematics;

namespace Features.Character.Services
{
    [Serializable]
    [BurstCompile]
    public struct AutoattackInfo
    {
        public int Id;
        public float Time;
        public float BaseDamage;

        public float2 CenterOffset;
        
        public float2 HorizontalOffset;
        public float2 VerticalOffset;

        public float2 HorizontalSize;
        public float2 VerticalSize;

        public float ColliderStartTime;
        public float ColliderStopTime;
        
        
    }
}