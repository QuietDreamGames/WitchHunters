using Features.Character.Components;
using Features.InputSystem.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.InputSystem.Systems
{
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<PlayerInputWrapper, PlayerInputConfiguration, Movement>()
                .ForEach(
                    (ref Movement movement, in PlayerInputWrapper input, in PlayerInputConfiguration conf) =>
                    {
                        movement.Value = input.Value.actions[conf.MoveActionID].ReadValue<Vector2>();
                    })
                .WithoutBurst()
                .Run();
        }
    }
}