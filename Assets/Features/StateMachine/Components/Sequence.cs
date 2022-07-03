using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components
{
	[BurstCompile]
	public struct Sequence : IComponentData, INode
	{
		public readonly Entity Entity;
		public readonly int NodeCount;
		
        public int CurrentNodeIndex;

        public Sequence(Entity entity, int nodeCount) : this()
        {
	        Entity = entity;
	        NodeCount = nodeCount;
        }
	}
}