using Unity.Entities;

namespace Features.StateMachine.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup), OrderFirst = true)]
    public partial class CompositeNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}