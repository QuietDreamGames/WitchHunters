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
                        var moveActionID = conf.MoveActionID.ToString();
                        var direction = new float3(input.Value.GetAxis(moveActionID));
                        
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
                .WithAll<CharacterInput, Attack>()
                .ForEach(
                    (CharacterInput input, ref Attack attack, in InputConfiguration conf) =>
                    {
                        var attackActionID = conf.AttackActionID.ToString();
                        var isAttackInput = input.Value.GetKey(attackActionID);

                        if (attack.Enable == isAttackInput)
                            return;
                        
                        attack.Enable = isAttackInput;
                        attack.Trigger = true;
                    })
                .WithoutBurst()
                .Run();
             
        }
    }
}