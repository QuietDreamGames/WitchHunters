using Features.Dungeon.Components;
using Unity.Entities;

namespace Features.Dungeon.Systems
{
    public partial class DungeonGeneratorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            RequireSingletonForUpdate<DungeonScene>();
            if (!HasSingleton<DungeonScene>())
            {
                return;
            }

            var dungeonScene = GetComponentFromSingleton<DungeonScene>();
            if (dungeonScene.Generated)
                return;
            
            RequireSingletonForUpdate<DungeonLoader>();
            if (!HasSingleton<DungeonLoader>())
            {
                return;
            }
            
            var dungeonLoader = GetComponentFromSingleton<DungeonLoader>();
            var dungeonGenerator = dungeonScene.Grid;

            dungeonGenerator.FixedLevelGraphConfig.LevelGraph = dungeonLoader.LevelGraph;
            dungeonGenerator.Generate();
            
            dungeonScene.Generated = true;
        }

        private T GetComponentFromSingleton<T>() where T : class, IComponentData
        {
            var entity = GetSingletonEntity<T>();
            var component = EntityManager.GetComponentData<T>(entity);
            return component;
        } 
    }
}