using Edgar.Unity;
using Unity.Entities;

namespace Features.Dungeon.Components
{
    public class DungeonScene : IComponentData
    {
        public DungeonGeneratorGrid2D Grid;
        public bool Generated;

        public DungeonScene()
        {
            Grid = default;
            Generated = false;
        }

        public DungeonScene(DungeonGeneratorGrid2D grid)
        {
            Grid = grid;
            Generated = false;
        }
    }
}