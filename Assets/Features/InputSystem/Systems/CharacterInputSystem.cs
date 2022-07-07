using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Components;
using Features.InputSystem.Services;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.InputSystem.Systems
{
    [UpdateBefore(typeof(MovementSystem))]
    public partial class CharacterInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<CharacterInput, Movement>()
                .ForEach(
                    (CharacterInput input, ref Movement movement, in InputConfiguration conf) =>
                    {
                        //var direction = new float3(input.Value.actions[conf.MoveActionID].ReadValue<Vector2>(), 0);
                        
                        var direction = new float3(input.Value.GetAxis(conf.MoveActionID));
                        movement.Enable = math.any(direction != float3.zero);
                        
                        if (movement.Enable)
                        {
                            movement.Direction = direction;
                        }
                    })
                .WithoutBurst()
                .Run();
            
            //Attack
            
            Entities
                .WithAll<CharacterInput, Movement>()
                .ForEach(
                    ( CharacterInput input, ref Movement movement, in InputConfiguration conf) =>
                    {
                        var direction = new float3(input.Value.GetAxis(conf.MoveActionID));
                        movement.Enable = math.any(direction != float3.zero);
                        
                        if (movement.Enable)
                        {
                            //movement.Direction = direction;
                        }
                    })
                .WithoutBurst()
                .Run();
             
        }
    }
}