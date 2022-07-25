using System;
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

        public float2 ColliderRectPos1;
        public float2 ColliderRectPos2;
        public float ColliderStartTime;
        public float ColliderStopTime;
        
        
    }
}