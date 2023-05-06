using System;
using Unity.Entities;
using UnityEngine;

namespace Presentation.Feature
{
    public class AnimatorConfComponent : IComponentData, IEquatable<AnimatorConfComponent>
    {
        public string Moving;

        public string Horizontal;
        public string Vertical;

        public bool Equals(AnimatorConfComponent other)
        {
            return Moving == other.Moving && Horizontal == other.Horizontal && Vertical == other.Vertical;
        }

        public override bool Equals(object obj)
        {
            return obj is AnimatorConfComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Moving, Horizontal, Vertical);
        }
    }
}
