using Unity.Entities;
using UnityEngine;

namespace Features.Character.Components
{
    public struct ContactFilter2DWrapper : IComponentData
    {
        public ContactFilter2D Value;

        public ContactFilter2DWrapper(ContactFilter2D value)
        {
            Value = value;
        }
    }
}