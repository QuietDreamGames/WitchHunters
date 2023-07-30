using Edgar.Unity;
using UnityEngine;

namespace Features.Dungeons.Initialization
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private DungeonGeneratorBaseGrid2D dungeonGenerator;
        [SerializeField] private RoomConfigurator roomConfigurator;
        
        [ContextMenu("Generate dungeon")]
        public void GenerateDungeon()
        {
            dungeonGenerator.Generate();
            roomConfigurator.ConfigureRoom();
        }
    }
}
