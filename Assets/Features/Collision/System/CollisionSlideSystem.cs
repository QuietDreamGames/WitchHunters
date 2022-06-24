using Features.Character.Components;
using Features.Character.Systems;
using Features.Collision.Utils;
using Features.InputSystem.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using RaycastHit = Unity.Physics.RaycastHit;

namespace Features.Collision.System
{
    [AlwaysUpdateSystem]
    [UpdateAfter(typeof(PlayerInputSystem))]
    [UpdateBefore(typeof(MovementSystem))]
    public partial class CollisionSlideSystem : SystemBase
    {
        private BuildPhysicsWorld _physicsWorld;
        private CollisionWorld _collisionWorld;

        protected override void OnCreate()
        {
            _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        protected override void OnUpdate()
        {
            _collisionWorld = _physicsWorld.PhysicsWorld.CollisionWorld;

            new CollisionJob
            {
                CollisionWorld = _collisionWorld,
                
                HalfCollider = 0.125f,
                DeltaTime = Time.DeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct CollisionJob : IJobEntity
    {
        [ReadOnly] public CollisionWorld CollisionWorld;

        public float HalfCollider;
        public float DeltaTime;

        private RaycastHit _raycastHit;
        
        public void Execute(in Translation translation, ref Movement movement, in Speed speed)
        {
            var rayStart = translation.Value;

            var (rayEndHorizontal1, rayEndHorizontal2) = GetRayEnd(rayStart, in movement, in speed, true);
            var (rayEndVertical1, rayEndVertical2) = GetRayEnd(rayStart, in movement, in speed, false);

            var moveX = Raycast(rayStart, rayEndHorizontal1, out _raycastHit) 
                        || Raycast(rayStart, rayEndHorizontal2, out _raycastHit);
            var moveY = Raycast(rayStart, rayEndVertical1, out _raycastHit) 
                        || Raycast(rayStart, rayEndVertical2, out _raycastHit);
            
            movement.Direction.x = moveX ? 0 : movement.Direction.x;
            movement.Direction.y = moveY ? 0 : movement.Direction.y;
        }

        private (float3, float3) GetRayEnd(float3 rayStart, in Movement movement, in Speed speed, bool horizontal)
        {
            var moveOneDirection = horizontal 
                ? new float3(movement.Direction.x, 0, 0) 
                : new float3(0, movement.Direction.y, 0);
            var offset = rayStart + moveOneDirection * speed.Value * DeltaTime;

            var direction = horizontal 
                ? math.sign(movement.Direction.x) 
                : math.sign(movement.Direction.y);

            var rayEnd1 = horizontal
                ? new float3(direction * HalfCollider, -HalfCollider, 0)
                : new float3(-HalfCollider, direction * HalfCollider, 0);
            var rayEnd2 = horizontal
                ? new float3(direction * HalfCollider, HalfCollider, 0)
                : new float3(HalfCollider, direction * HalfCollider, 0);

            rayEnd1 += offset;
            rayEnd2 += offset;

            return (rayEnd1, rayEnd2);
        }

        private bool Raycast(float3 rayStart, float3 rayEnd, out RaycastHit raycastHit)
        {
            var raycastInput = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter
                {
                    BelongsTo = (uint) CollisionLayers.Player,
                    CollidesWith = (uint) (CollisionLayers.Ground | CollisionLayers.Enemy)
                }
            };

            var result = CollisionWorld.CastRay(raycastInput, out raycastHit);
            return result;
        }
    }
}
