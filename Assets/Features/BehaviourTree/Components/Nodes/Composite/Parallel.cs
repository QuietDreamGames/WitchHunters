using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Composite
{
    [BurstCompile]
    public struct Parallel : IComponentData, INode
    {
        public readonly int NodeCount;
		
        public readonly ParallelType Type;

        public Parallel(int nodeCount, ParallelType type) : this()
        {
            NodeCount = nodeCount;
            Type = type;
        } 
    }

    public enum ParallelType
    {
        Simple = 0,
        Sequence,
        Selector
    }
}