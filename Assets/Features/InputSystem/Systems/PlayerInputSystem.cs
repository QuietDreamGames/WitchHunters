using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.InputSystem.Systems
{
    [UpdateBefore(typeof(CharacterInputSystem))]
    public partial class PlayerInputSystem : SystemBase
    {
        // private PlayerInputComponent _playerInputComponent;
        //     
        // protected override void OnStartRunning()
        // {
        //     RequireSingletonForUpdate<PlayerInputComponent>();
        //     var playerInputEntity = GetSingletonEntity<PlayerInputComponent>();
        //     _playerInputComponent = EntityManager.GetComponentData<PlayerInputComponent>(playerInputEntity);
        // }

        protected override void OnUpdate()
        {
            var playerInputEntity = GetSingletonEntity<PlayerInputComponent>();
            var playerInputComponent = EntityManager.GetComponentData<PlayerInputComponent>(playerInputEntity);
            
            Entities
                .WithAll<CharacterInput, InputConfiguration, PlayerTag>()
                .ForEach(
                    (CharacterInput characterInput, in InputConfiguration conf) =>
                    {
                        var direction =
                            new float3(playerInputComponent.Value.actions[conf.MoveActionID].ReadValue<Vector2>(), 0);

                        var isAttack = playerInputComponent.Value.actions[conf.AttackActionID].IsPressed();

                        characterInput.Value.SetAxis(conf.MoveActionID, direction);
                        
                        characterInput.Value.SetKey(conf.AttackActionID, isAttack);
                        
                    })
                .WithoutBurst()
                .Run();
        }
    }
}