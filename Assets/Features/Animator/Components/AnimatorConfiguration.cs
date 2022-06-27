using System;
using Unity.Entities;
using UnityEngine;

namespace Features.Animator.Components
{
    public struct AnimatorConfiguration : ISharedComponentData, IEquatable<AnimatorConfiguration>
    {
        public string Moving;

        public string Horizontal;
        public string Vertical;

        public bool Equals(AnimatorConfiguration other)
        {
            return Moving == other.Moving && Horizontal == other.Horizontal && Vertical == other.Vertical;
        }

        public override bool Equals(object obj)
        {
            return obj is AnimatorConfiguration other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Moving, Horizontal, Vertical);
        }
    }
}
