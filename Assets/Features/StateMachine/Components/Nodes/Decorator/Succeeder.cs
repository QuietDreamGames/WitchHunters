using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Decorator
{
    [BurstCompile]
    public struct Succeeder : IComponentData, INode
    {
        
    }
}