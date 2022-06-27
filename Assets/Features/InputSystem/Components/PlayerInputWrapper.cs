using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.InputSystem.Components
{
    public struct PlayerInputWrapper : ISharedComponentData, IEquatable<PlayerInputWrapper>
    {
        public PlayerInput Value;

        public bool Equals(PlayerInputWrapper other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerInputWrapper other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
