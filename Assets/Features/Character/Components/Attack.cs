using Unity.Entities;

namespace Features.Character.Components
{
    public struct Attack : IComponentData
    {
        public bool Enable;
        public int AttackId;
    }
}