using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Composite
{
	[BurstCompile]
	public struct Sequence : IComponentData, INode
	{
		public readonly int NodeCount;
		
        public int CurrentNodeIndex;

        public Sequence(int nodeCount) : this()
        {
	        NodeCount = nodeCount;
        }
	}
}