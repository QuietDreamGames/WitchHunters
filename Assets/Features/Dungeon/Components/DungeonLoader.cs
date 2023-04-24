using Edgar.Unity;
using Unity.Entities;

namespace Features.Dungeon.Components
{
    public class DungeonLoader : IComponentData
    {
        public LevelGraph LevelGraph;

        public DungeonLoader()
        {
            LevelGraph = default;
        }

        public DungeonLoader(LevelGraph levelGraph)
        {
            LevelGraph = levelGraph;
        }
    }
}