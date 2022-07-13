using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Composite
{
    [BurstCompile]
    public struct Selector : IComponentData, INode
    {
        public readonly int NodeCount;
		
        public int CurrentNodeIndex;

        public Selector(int nodeCount) : this()
        {
            NodeCount = nodeCount;
        }
    }
}