using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Character.Services
{
    [Serializable]
    public struct AutoattackInfo
    {
        public int Id;
        public float Time;
        public float BaseDamage;

        public float ColliderWidth;
        public float ColliderHeight;

        public float2 OffsetXY;

        public float ColliderStartTime;
        public float ColliderStopTime;
        
        
    }
}