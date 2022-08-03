using Features.Dungeon.Components;
using Unity.Entities;

namespace Features.Dungeon.Systems
{
    public partial class DungeonGeneratorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!HasSingleton<DungeonScene>())
            {
                return;
            }

            var (dungeonSceneEntity, dungeonScene) = GetComponentFromSingleton<DungeonScene>();

            if (!HasSingleton<DungeonLoader>())
            {
                return;
            }
            
            var (_, dungeonLoader) = GetComponentFromSingleton<DungeonLoader>();
            var dungeonGenerator = dungeonScene.Grid;

            dungeonGenerator.FixedLevelGraphConfig.LevelGraph = dungeonLoader.LevelGraph;
            dungeonGenerator.Generate();
            
            EntityManager.DestroyEntity(dungeonSceneEntity);
        }

        private (Entity, T) GetComponentFromSingleton<T>() where T : class, IComponentData
        {
            var entity = GetSingletonEntity<T>();
            var component = EntityManager.GetComponentData<T>(entity);
            return (entity, component);
        } 
    }
}