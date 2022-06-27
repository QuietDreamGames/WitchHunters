using Unity.Entities;

namespace Features.HealthSystem.Components
{
    public struct Damage : IComponentData
    {
        public float Value;
        public bool Enable;
    }
}