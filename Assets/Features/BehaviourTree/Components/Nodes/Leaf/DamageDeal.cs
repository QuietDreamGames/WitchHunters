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
        public float Width;
        public float Height;
        public float2 OffsetXY;

        public float Cooldown;
        public float Damage;

        public float StartTime;
        public float StopTime;

        public float CurrentTime;

        public DamageDeal(AutoattackInfo attack) : this()
        {
            Width = attack.ColliderWidth;
            Height = attack.ColliderHeight;
            OffsetXY = attack.OffsetXY;

            Cooldown = attack.Time;
            Damage = attack.BaseDamage;

            StartTime = attack.ColliderStartTime;
            StopTime = attack.ColliderStopTime;
        }
    }
}