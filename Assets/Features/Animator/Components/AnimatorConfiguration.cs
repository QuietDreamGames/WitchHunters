using Unity.Collections;
using Unity.Entities;

namespace Features.Animator.Components
{
    public struct AnimatorConfiguration : IComponentData
    {
        public FixedString32Bytes Moving;

        public FixedString32Bytes Horizontal;
        public FixedString32Bytes Vertical;

        public FixedString32Bytes Attack;

    }
}
