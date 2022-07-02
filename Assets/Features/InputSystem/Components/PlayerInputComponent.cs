using Unity.Entities;
using UnityEngine.InputSystem;

namespace Features.InputSystem.Components
{
    [GenerateAuthoringComponent]
    public class PlayerInputComponent : IComponentData
    {
        public PlayerInput Value;
    }
}