using Unity.Entities;

namespace Features.HealthSystem.Components
{
    [GenerateAuthoringComponent]
    public struct Health : IComponentData
    {
        public float InitialValue;
        public float Value;
    }
}