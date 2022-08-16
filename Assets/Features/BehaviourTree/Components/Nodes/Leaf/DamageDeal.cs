using Features.BehaviourTree.Components.Core;
using Features.Character.Services;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct DamageDeal : IComponentData, INode
    {
        public float2 CenterOffset;
        
        public float2 HorizontalOffset;
        public float2 VerticalOffset;

        public float2 HorizontalSize;
        public float2 VerticalSize;

        public float Cooldown;
        public float Damage;

        public float StartTime;
        public float StopTime;

        public float CurrentTime;

        public DamageDeal(AutoattackInfo attack) : this()
        {
            CenterOffset = attack.CenterOffset;
            
            HorizontalOffset = attack.HorizontalOffset;
            VerticalOffset = attack.VerticalOffset;

            HorizontalSize = attack.HorizontalSize;
            VerticalSize = attack.VerticalSize;

            Cooldown = attack.Time;
            Damage = attack.BaseDamage;

            StartTime = attack.ColliderStartTime;
            StopTime = attack.ColliderStopTime;
        }
    }
}