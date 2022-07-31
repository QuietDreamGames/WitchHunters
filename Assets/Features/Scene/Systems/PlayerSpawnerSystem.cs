using Features.Character.Components;
using Features.Scene.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Features.Scene.Systems
{
    public partial class PlayerSpawnerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            RequireSingletonForUpdate<PlayerSpawner>();
            if (!HasSingleton<PlayerSpawner>())
            {
                return;
            }

            const uint multiplier = 5000000; 
            var seed = (uint)((UnityEngine.Random.value + 1) * multiplier); 
            var random = new Random(seed);
            
            var (entity, playerSpawner) = GetComponentFromSingleton<PlayerSpawner>();

            var playerQuery = EntityManager.CreateEntityQuery(
                ComponentType.ReadOnly<PlayerTag>(),
                ComponentType.ReadWrite<Translation>());

            var setPositionToEntityJob = new SetPositionToEntityJob
            {
                PlayerSpawner = playerSpawner,
                Random = random,
            }.ScheduleParallel(playerQuery);
            
            setPositionToEntityJob.Complete();
            EntityManager.DestroyEntity(entity);

            new TranslationToComponentFollowerJob{}.Run();
        }
        
        [BurstCompile]
        private partial struct SetPositionToEntityJob : IJobEntity
        {
            public PlayerSpawner PlayerSpawner;

            public Random Random;
            
            public void Execute(ref Translation translation)
            {
                var offset = new float2
                {
                    x = Random.NextFloat(-PlayerSpawner.Size.x / 2,
                        PlayerSpawner.Size.x),
                    y = Random.NextFloat(-PlayerSpawner.Size.y / 2,
                        PlayerSpawner.Size.y),
                };

                var position = new float3
                {
                    x = PlayerSpawner.Center.x + offset.x,
                    y = PlayerSpawner.Center.y + offset.y,
                    z = PlayerSpawner.Center.z
                };

                translation.Value = position;
            }
        }
        
        private partial struct TranslationToComponentFollowerJob : IJobEntity
        {
            public void Execute(in Translation translation, in CompanionFollower companion)
            {
                companion.Value.position = translation.Value;
            }
        } 
        
        private (Entity, T) GetComponentFromSingleton<T>() where T : struct, IComponentData
        {
            var entity = GetSingletonEntity<T>();
            var component = EntityManager.GetComponentData<T>(entity);
            return (entity, component);
        } 
    }
}