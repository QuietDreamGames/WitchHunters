using Features.InputSystem.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace Features.InputSystem.Systems
{
    public partial class InputMapChangeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // var playerInputEntity = GetSingletonEntity<PlayerInputComponent>();
            // var playerInputComponent = EntityManager.GetComponentData<PlayerInputComponent>(playerInputEntity);
            // var ecb = new EntityCommandBuffer(Allocator.TempJob);
            //
            // var applyDamageJob = new ChangeInputMapJob
            //     { EntityCommandBuffer = ecb, PlayerInput = playerInputComponent.Value }.Schedule();
            // applyDamageJob.Complete();
            // ecb.Playback(EntityManager);
            // ecb.Dispose();
        }

        public partial struct ChangeInputMapJob : IJobEntity
        {
            // public EntityCommandBuffer EntityCommandBuffer;
            // public PlayerInput PlayerInput;
            //
            // private void Execute(Entity e, in ChangeInputMapFlag changeInputMapFlag)
            // {
            //     switch (changeInputMapFlag.NewInputMapId)
            //     {
            //         case 0:
            //             PlayerInput.SwitchCurrentActionMap("Player");
            //             break;
            //         case 1:
            //             PlayerInput.SwitchCurrentActionMap("UI");
            //             break;
            //     }
            //     
            //     EntityCommandBuffer.DestroyEntity(e);
            // }
        }
    }
}