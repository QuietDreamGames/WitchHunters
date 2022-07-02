using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class InputConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            
        }
    }
}