using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Leaf
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