using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.InputSystem.Systems
{
    [UpdateBefore(typeof(MovementSystem))]
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<PlayerInputWrapper, PlayerInputConfiguration, Movement>()
                .ForEach(
                    (ref Movement movement, in PlayerInputWrapper input, in PlayerInputConfiguration conf) =>
                    {
                        float2 direction = input.Value.actions[conf.MoveActionID].ReadValue<Vector2>();
                        movement.Enable = math.any(direction != float2.zero);
                        
                        if (movement.Enable)
                        {
                            movement.Direction = direction;
                        }
                    })
                .WithoutBurst()
                .Run();
        }
    }
}