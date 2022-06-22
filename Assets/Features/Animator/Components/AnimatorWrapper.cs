using System;
using Unity.Entities;

namespace Features.Animator.Components
{
    public struct AnimatorWrapper : ISharedComponentData, IEquatable<AnimatorWrapper>
    {
        public UnityEngine.Animator Value;

        public bool Equals(AnimatorWrapper other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is AnimatorWrapper other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
