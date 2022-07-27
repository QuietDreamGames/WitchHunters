using System;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace Features.Test.Services
{
    public struct TestInputWrapper : ISharedComponentData, IEquatable<TestInputWrapper>
    {
        public PlayerInput Value;
        
        public bool Equals(TestInputWrapper other)
        {
            return Equals(Value, other.Value);
        }
        
        public override bool Equals(object obj)
        {
            return obj is TestInputWrapper other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}