using Features.InputSystem.Systems;
using Unity.Entities;

namespace Features.Character.Systems.SystemGroups
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(LateSimulationSystemGroup))]
    public class GameObjectSyncGroup : ComponentSystemGroup
    {
        
    }
}