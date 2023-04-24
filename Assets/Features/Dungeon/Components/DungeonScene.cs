using Edgar.Unity;
using Unity.Entities;

namespace Features.Dungeon.Components
{
    public class DungeonScene : IComponentData
    {
        public DungeonGeneratorGrid2D Grid;

        public DungeonScene()
        {
            Grid = default;
        }

        public DungeonScene(DungeonGeneratorGrid2D grid)
        {
            Grid = grid;
        }
    }
}