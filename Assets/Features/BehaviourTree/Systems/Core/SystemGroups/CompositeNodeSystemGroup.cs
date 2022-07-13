using Unity.Entities;

namespace Features.BehaviourTree.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup), OrderFirst = true)]
    public partial class CompositeNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}