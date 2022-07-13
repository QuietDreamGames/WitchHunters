using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.Animator.Components
{
    [BurstCompile]
    public struct PlayNextAnimation : IComponentData
    {
        public FixedString32Bytes AnimationName;
        public bool Enable;
    }
}