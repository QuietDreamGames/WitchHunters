using Unity.Entities;

namespace Features.StateMachine.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(VariableRateSimulationSystemGroup))]
    public partial class StateMachineSystemGroup : ComponentSystemGroup
    {
        
    }
}