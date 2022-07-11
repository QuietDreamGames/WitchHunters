using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Leaf
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