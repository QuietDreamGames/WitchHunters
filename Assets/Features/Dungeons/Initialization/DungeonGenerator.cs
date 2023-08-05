using System.Collections;
using Edgar.Unity;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using Pathfinding;
using UnityEngine;

namespace Features.Dungeons.Initialization
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private DungeonGeneratorBaseGrid2D dungeonGenerator;
        [SerializeField] private RoomConfigurator roomConfigurator;
        [SerializeField] private AstarPath astarPath;
        
        [SerializeField] private Seeker seeker;

        [ContextMenu("Generate dungeon")]
        public void GenerateDungeon()
        {
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
