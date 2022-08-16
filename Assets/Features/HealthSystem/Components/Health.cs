using Unity.Entities;

namespace Features.HealthSystem.Components
{
    [GenerateAuthoringComponent]
    public struct Health : IComponentData
    {
        public float MaxValue;
        public float Value;
        
        public Health(float maxValue)
        {
            MaxValue = maxValue;
            Value = maxValue;
        }
    }
}