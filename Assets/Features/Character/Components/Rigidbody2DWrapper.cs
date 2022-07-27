using Unity.Entities;
using UnityEngine;

namespace Features.Character.Components
{
    public class Rigidbody2DWrapper : IComponentData
    {
        public Rigidbody2D Value;

        public Rigidbody2DWrapper()
        {
            Value = default;
        }
        
        public Rigidbody2DWrapper(Rigidbody2D value)
        {
            Value = value;
        }
    }
}