using Unity.Entities;

namespace Features.HealthSystem.Components
{
    [InternalBufferCapacity(16)]
    public struct Damage : IBufferElementData
    {
        public int SourceEntityId;
        public bool Enabled;
        public float Value;
        public float Cooldown; // cooldown of invisibility from this damage source
    }
}