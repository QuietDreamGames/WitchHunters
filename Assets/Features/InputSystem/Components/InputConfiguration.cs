using System;
using Unity.Collections;
using Unity.Entities;

namespace Features.InputSystem.Components
{
    public struct InputConfiguration : IComponentData
    {
        public FixedString32Bytes MoveActionID;
        public FixedString32Bytes AttackActionID;

        public InputConfiguration(string moveActionID, string attackActionID)
        {
            MoveActionID = moveActionID;
            AttackActionID = attackActionID;
        }
    }
}
