using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Composite
{
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