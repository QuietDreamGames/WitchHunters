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

        private CollisionFilter _friendlyFilter;
        private CollisionFilter _enemyFilter;

        private EntityQuery _friendlyQuery;
        private EntityQuery _enemyQuery;

        protected override void OnCreate()
        {
            _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            
            _friendlyFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Player,
                CollidesWith = (uint)CollisionLayers.Enemy
            };
            
            _enemyFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Enemy,
                CollidesWith = (uint)CollisionLayers.Player
            };

            var friendlyQueryDesc = new EntityQueryDesc
            {
                //None = new ComponentType[] { typeof(Enemy) },
                Any = new ComponentType[] { typeof(PlayerTag)}, //, ComponentType.ReadOnly<FriendlyTag>() },
                All = new ComponentType[] { typeof(AttackOverlapBox), typeof(Attack), ComponentType.ReadOnly<Translation>() }
            };
            _friendlyQuery = GetEntityQuery(friendlyQueryDesc);
            
            var enemyQueryDesc = new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(PlayerTag) }, //, ComponentType.ReadOnly<FriendlyTag>() },
                //Any = new ComponentType[] { typeof(Enemy) },
                All = new ComponentType[] { typeof(AttackOverlapBox), typeof(Attack), ComponentType.ReadOnly<Translation>() }
                
            };
            _enemyQuery = GetEntityQuery(enemyQueryDesc);
        }
        
        protected override void OnUpdate()
        {
            _collisionWorld = _physicsWorld.PhysicsWorld.CollisionWorld;

            var attackAllEnemies = new AttackAllEntitiesInOverlapBox
            {
                CollisionWorld = _collisionWorld, 
                EntityManager = EntityManager,
                Filter = _friendlyFilter
            }.Schedule(_friendlyQuery);
            
            var attackAllFriends = new AttackAllEntitiesInOverlapBox
            {
                CollisionWorld = _collisionWorld, 
                EntityManager = EntityManager,
                Filter = _enemyFilter
            }.Schedule(_enemyQuery);
            
            attackAllEnemies.Complete();
            attackAllFriends.Complete();
        }
        
        [BurstCompile]
        public partial struct AttackAllEntitiesInOverlapBox : IJobEntity
        {
            [ReadOnly] public CollisionWorld CollisionWorld;
            public EntityManager EntityManager;
            public CollisionFilter Filter;
            
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

                var aabbInput = new OverlapAabbInput
                {
                    Aabb = aabb,
                    Filter = Filter
                };

                var hits = new NativeList<int>(16, Allocator.Temp);
                
                var result = CollisionWorld.OverlapAabb(aabbInput, ref hits);

                if (!result) return;
                
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
                        Cooldown = 0.1f
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