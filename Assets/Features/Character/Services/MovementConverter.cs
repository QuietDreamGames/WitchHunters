using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class MovementConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("GO Movement")] 
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private ContactFilter2D _filter;
        
        [Header("Speed Data")] 
        [SerializeField] private float _speed = 2f;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var rigidbody2DWrapper = new Rigidbody2DWrapper(_rigidbody2D);
            dstManager.AddComponentData(entity, rigidbody2DWrapper);

            var filterWrapper = new ContactFilter2DWrapper(_filter);
            dstManager.AddComponentData(entity, filterWrapper);

            var speed = new Speed(_speed);
            dstManager.AddComponentData(entity, speed);
        }

        #endregion
    }
}
