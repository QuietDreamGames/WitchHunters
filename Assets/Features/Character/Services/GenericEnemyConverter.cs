using Features.Animator.Components;
using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class GenericEnemyConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Movement());
            dstManager.AddComponentData(entity, new Speed());
            
            dstManager.AddComponentData(entity, new Target());

            dstManager.AddComponentData(entity, new PlayNextAnimation());
        }
    }
}
