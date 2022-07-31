using Edgar.Unity;
using Features.Dungeon.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Test.Services
{
    public class TestDungeonLoaderConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private LevelGraph _levelGraph;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var dungeonLoader = new DungeonLoader(_levelGraph);
            dstManager.AddComponentData(entity, dungeonLoader);
        }
    }
}