using Features.Character.Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace Features.Character.Systems
{
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

            var aabb = new Aabb();

            var autoAttackQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadWrite<Autoattacks>(),
                ComponentType.ReadWrite<Attack>());
        }
    }
}