using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct GetClosestPlayer : IComponentData, INode
    {
        public readonly float Distance;

        public GetClosestPlayer(float distance) : this()
        {
            Distance = distance;
        }
    }
}