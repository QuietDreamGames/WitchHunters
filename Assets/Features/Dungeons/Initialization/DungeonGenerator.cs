using System.Collections;
using Edgar.Unity;
using Features.Character.Spawn;
using Features.Progression;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Dungeons.Initialization
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private DungeonGeneratorBaseGrid2D dungeonGenerator;
        [SerializeField] private RoomConfigurator roomConfigurator;
        [SerializeField] private LevelGraphConfigurator levelGraphConfigurator;
        [SerializeField] private AstarPath astarPath;
        
        [ContextMenu("Generate dungeon")]
        public void GenerateDungeon()
        {
            levelGraphConfigurator.ConfigureLevelGraph();
            dungeonGenerator.Generate();
            roomConfigurator.ConfigureRoom();
            
            StartCoroutine(ScanCoroutine());
        }
        
        private IEnumerator ScanCoroutine()
        {
            yield return null;
            astarPath.Scan();
        }

        private void Start()
        {
            GenerateDungeon();
            
            var characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            characterHolder.CurrentCharacter.Restart();
        }
    }
}
