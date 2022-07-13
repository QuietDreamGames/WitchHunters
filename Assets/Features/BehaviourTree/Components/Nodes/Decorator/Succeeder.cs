using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Decorator
{
    [BurstCompile]
    public struct Succeeder : IComponentData, INode
    {
        
    }
}