using Unity.Entities;

namespace Features.StateMachine.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup))]
    public partial class DecoratorNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}