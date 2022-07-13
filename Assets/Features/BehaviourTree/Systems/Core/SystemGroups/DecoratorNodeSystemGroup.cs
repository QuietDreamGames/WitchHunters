using Unity.Entities;

namespace Features.BehaviourTree.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup))]
    public partial class DecoratorNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}