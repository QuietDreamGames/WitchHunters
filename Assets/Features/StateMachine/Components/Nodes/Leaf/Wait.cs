using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct Wait : IComponentData, INode
    {
        public readonly float WaitTime;

        public float CurrentWaitTime;

        public Wait(float waitTime) : this()
        {
            WaitTime = waitTime;
        }
    }
}