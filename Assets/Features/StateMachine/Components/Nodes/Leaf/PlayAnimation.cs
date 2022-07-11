using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct PlayAnimation : IComponentData, INode
    {
        public readonly FixedString32Bytes AnimationName;

        public PlayAnimation(string animationName) : this()
        {
            AnimationName = animationName;
        }
    }
}