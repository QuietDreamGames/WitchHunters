using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Leaf
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