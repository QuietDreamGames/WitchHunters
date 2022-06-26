using System;

using Unity.Entities;

namespace Features.Test.Services
{
    public struct TestInputConfiguration :  ISharedComponentData, IEquatable<TestInputConfiguration>
    {
        public string ApplyDamageActionID;

        public bool Equals(TestInputConfiguration other)
        {
            return ApplyDamageActionID == other.ApplyDamageActionID;
        }
        
        public override bool Equals(object obj)
        {
            return obj is TestInputConfiguration other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ApplyDamageActionID);
        }
    }
}