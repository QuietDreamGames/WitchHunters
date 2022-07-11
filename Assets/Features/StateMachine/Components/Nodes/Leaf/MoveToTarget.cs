using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct MoveToTarget : IComponentData, INode
    {
        public readonly float AcceptableRadius;
        public readonly float Speed;

        public MoveToTarget(float acceptableRadius, float speed) : this()
        {
            AcceptableRadius = acceptableRadius;
            Speed = speed;
        }
    }
}