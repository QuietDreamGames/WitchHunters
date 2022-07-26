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

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var rigidbody2DWrapper = new Rigidbody2DWrapper(_rigidbody2D);
            dstManager.AddComponentData(entity, rigidbody2DWrapper);
        }

        #endregion
    }
}
