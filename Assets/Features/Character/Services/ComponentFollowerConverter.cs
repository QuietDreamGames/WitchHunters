using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class ComponentFollowerConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Companion Follower")] 
        [SerializeField] private Transform _companion;

        #endregion
        
        #region IConvertGameObjectToEntity implementation
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var companion = new CompanionFollower(_companion);
            dstManager.AddComponentData(entity, companion);
        }
        
        #endregion
    }
}
