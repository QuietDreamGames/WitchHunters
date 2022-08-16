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
                        var moveActionID = conf.MoveActionID.ToString();
                        var direction2D = playerInputComponent.Value.actions[moveActionID].ReadValue<Vector2>();
                        var direction3D = new float3(direction2D, 0);

                        var attackActionID = conf.AttackActionID.ToString();
                        var isAttack = playerInputComponent.Value.actions[attackActionID].IsPressed();

                        characterInput.Value.SetAxis(moveActionID, direction3D);
                        characterInput.Value.SetKey(attackActionID, isAttack);
                    })
                .WithoutBurst()
                .Run();
        }
    }
}