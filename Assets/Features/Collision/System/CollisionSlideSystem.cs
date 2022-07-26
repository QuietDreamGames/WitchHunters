using Features.Character.Components;
using Features.Character.Systems;
using Features.InputSystem.Systems;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Collision.System
{
    [AlwaysUpdateSystem]
    [UpdateAfter(typeof(PlayerInputSystem))]
    [UpdateBefore(typeof(MovementSystem))]
    public partial class CollisionSlideSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new CollisionJob{}.Run();
        }
    }
    
    public partial struct CollisionJob : IJobEntity
    {
        private RaycastHit _raycastHit;

        public void Execute(Rigidbody2DWrapper rigidbody2D,
            in ContactFilter2DWrapper contactFilter2D,
            ref Movement movement)
        {
            if (!movement.Enable)
                return;

            var collisions = new RaycastHit2D[100];
            const float distance = 0.0175f; 
            var moveX = Raycast(movement.Direction.xz,
                distance,
                rigidbody2D.Value,
                contactFilter2D.Value,
                collisions);
            var moveY = Raycast(movement.Direction.zy,
                distance,
                rigidbody2D.Value,
                contactFilter2D.Value,
                collisions);
            
            movement.Direction.x = !moveX ? 0 : movement.Direction.x;
            movement.Direction.y = !moveY ? 0 : movement.Direction.y;
            movement.Direction = math.normalizesafe(movement.Direction, float3.zero);
            movement.Enable = math.any(movement.Direction != float3.zero);
        }

        private bool Raycast(float2 direction,
            float distance,
            Rigidbody2D rigidbody2D,
            ContactFilter2D filter,
            RaycastHit2D[] collisions)
        {
            var isZero = math.all(direction == float2.zero);
            if (isZero)
                return true;
            
            var count = rigidbody2D.Cast(direction,
                filter,
                collisions,
                distance);

            return count == 0;
        }
    }
}
