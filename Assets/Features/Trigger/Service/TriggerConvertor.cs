using Unity.Entities;
using UnityEngine;

namespace Features.Trigger.Service
{
    public class TriggerConvertor : MonoBehaviour, IConvertGameObjectToEntity
    {
        
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            throw new System.NotImplementedException();
        }
    }
}