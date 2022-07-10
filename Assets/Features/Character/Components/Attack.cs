using Unity.Entities;

namespace Features.Character.Components
{
    public struct Attack : IComponentData
    {
        public bool Enable;
        public float Cooldown;
        public int CurrentAttackId;
        public int NextAttackId;
        public float Damage;
    }
}