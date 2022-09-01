using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class TimeConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var deltaTime = new DeltaTime();
            dstManager.AddComponentData(entity, deltaTime);
        }
    }
}