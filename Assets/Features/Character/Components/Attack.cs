using Unity.Burst;
using Unity.Entities;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct Attack : IComponentData
    {
        public bool Enable;
        public bool Trigger;
        
        public float Cooldown;
        public int CurrentAttackId;
        public int NextAttackId;

        public bool IsAttackCollider;
        public float Damage;
        
    }
}