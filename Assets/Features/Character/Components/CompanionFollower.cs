using System;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Components
{
    public class CompanionFollower : IComponentData
    {
        public Transform Value;

        public CompanionFollower()
        {
            Value = default;
        }

        public CompanionFollower(Transform value)
        {
            Value = value;
        }
    }
}
