using Edgar.Unity;
using Features.Dungeon.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Dungeon.Services
{
    public class DungeonSceneConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private DungeonGeneratorGrid2D _grid;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var dungeonScene = new DungeonScene(_grid);
            dstManager.AddComponentData(entity, dungeonScene);
        }
    }
}