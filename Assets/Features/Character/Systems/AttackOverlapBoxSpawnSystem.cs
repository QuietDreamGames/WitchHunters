using Features.Character.Components;
using Features.Collision.System;
using Features.Collision.Utils;
using Features.HealthSystem.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Features.Character.Systems
{
    [UpdateAfter(typeof(CollisionSlideSystem))]
    [UpdateAfter(typeof(AttackSystem))]
    public partial class AttackOverlapBoxSpawnSystem : SystemBase
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

            var job = new AttackAllEntitiesInOverlapBox
            {
                CollisionWorld = _collisionWorld, 
                EntityManager = EntityManager
            }.Schedule();
            
            job.Complete();
        }
        
        [BurstCompile]
        public partial struct AttackAllEntitiesInOverlapBox : IJobEntity
        {
            [ReadOnly] public CollisionWorld CollisionWorld;
            public EntityManager EntityManager;
            
            private void Execute(ref AttackOverlapBox attackOverlapBox, in Translation translation, ref Attack attack)
            {
                if (!attackOverlapBox.Enable)
                    return;
                
                var localMinX = attackOverlapBox.OffsetXY.x - attackOverlapBox.Width / 2f;
                var localMinY = attackOverlapBox.OffsetXY.y - attackOverlapBox.Height / 2f;
                var localMinXY = new float3(localMinX, localMinY, 0f);
                
                var localMaxX = attackOverlapBox.OffsetXY.x + attackOverlapBox.Width / 2f;
                var localMaxY = attackOverlapBox.OffsetXY.y + attackOverlapBox.Height / 2f;
                var localMaxXY = new float3(localMaxX,  localMaxY, 0f);
                
                var aabb = new Aabb()
                {
                    Min = new float3(localMinXY + translation.Value),
                    Max = new float3(localMaxXY + translation.Value)
                };

                //Debug.Log($"Attack min coords:{aabb.Min}, attack max coords: {aabb.Max}");

                var filter = new CollisionFilter
                {
                    BelongsTo = (uint)CollisionLayers.Player,
                    CollidesWith = (uint)CollisionLayers.Enemy
                };
                
                var aabbInput = new OverlapAabbInput
                {
                    Aabb = aabb,
                    Filter = filter
                };

                var hits = new NativeList<int>(16, Allocator.Temp);
                
                var result = CollisionWorld.OverlapAabb(aabbInput, ref hits);
                
                if (!result)
                {
                    //Debug.Log("no hits");
                    return;
                }
                
                foreach (var entityId in hits)
                {
                    var entity = CollisionWorld.Bodies[entityId].Entity;
                    if (!EntityManager.HasComponent<DamageableTag>(entity)) continue;
                    
                    var damages = EntityManager.GetBuffer<Damage>(entity);
                    if (!IsDamageSourceNew(damages, entityId)) continue;
                    
                    var damage = new Damage
                    {
                        SourceEntityId = entityId, 
                        Value = attack.Damage, 
                        Enabled = true, 
                        Cooldown = 1f
                    };

                    damages.Add(damage);
                }

            }
            
            private bool IsDamageSourceNew(DynamicBuffer<Damage> damages, int entityId)
            {
                for (var index = 0; index < damages.Length; index++)
                {
                    var damage = damages[index];
                    if (damage.SourceEntityId != entityId) continue;
                    return false;
                }
                return true;
            }
        }

        
    }
}