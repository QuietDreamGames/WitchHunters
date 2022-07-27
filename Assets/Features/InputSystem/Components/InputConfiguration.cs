using System;
using Unity.Entities;

namespace Features.InputSystem.Components
{
    public struct InputConfiguration : ISharedComponentData, IEquatable<InputConfiguration>
    {
        public string MoveActionID;
        public string AttackActionID;

        public bool Equals(InputConfiguration other)
        {
            return MoveActionID == other.MoveActionID && AttackActionID == other.AttackActionID;
        }

        public override bool Equals(object obj)
        {
            return obj is InputConfiguration other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MoveActionID, AttackActionID);
        }
    }
}
