using Edgar.Unity;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
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

        private void Start()
        {
            GenerateDungeon();
            
            var characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            characterHolder.CurrentCharacter.Restart();
        }
    }
}
