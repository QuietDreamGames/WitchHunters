using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Systems.Core
{
    [UpdateAfter(typeof(SequenceRunnerSystem))]
    public partial class SequenceIteratorSystem : SystemBase 
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref NodeAgent agent) =>
                {
                    if (agent.LastResult == NodeResult.Success)
                    {
                        agent.CurrentNodeIndex = (agent.CurrentNodeIndex + 1) % agent.NodeCount;
                    }
                })
                .ScheduleParallel();
        }
    }
}