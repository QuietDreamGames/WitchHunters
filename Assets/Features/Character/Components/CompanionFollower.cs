using System;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Components
{
    public struct CompanionFollower : ISharedComponentData, IEquatable<CompanionFollower>
    {
        public Transform Value;

        public bool Equals(CompanionFollower other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is CompanionFollower other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
