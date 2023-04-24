using Unity.Burst;
using Unity.Entities;

namespace Features.Trigger.Components
{
    [BurstCompile]
    public struct TriggerInteractable : IComponentData
    {
        public bool Value;
    }
}