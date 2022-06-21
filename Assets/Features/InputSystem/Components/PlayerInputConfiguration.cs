using System;
using Unity.Entities;

namespace Features.InputSystem.Components
{
    public struct PlayerInputConfiguration : ISharedComponentData, IEquatable<PlayerInputConfiguration>
    {
        public string MoveActionID;
        public string AttackActionID;

        public bool Equals(PlayerInputConfiguration other)
        {
            return MoveActionID == other.MoveActionID && AttackActionID == other.AttackActionID;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerInputConfiguration other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MoveActionID, AttackActionID);
        }
    }
}
