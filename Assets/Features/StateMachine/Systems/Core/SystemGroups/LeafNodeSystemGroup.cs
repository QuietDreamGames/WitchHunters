using Unity.Entities;

namespace Features.StateMachine.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup), OrderLast = true)]
    public partial class LeafNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}