using Unity.Entities;

namespace Features.Trigger.Components
{
    public struct InTrigger : IComponentData
    {
        public bool IsInTrigger;
        public int TriggerType;
    }
}