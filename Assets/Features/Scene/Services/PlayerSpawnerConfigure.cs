using Features.Scene.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Scene.Services
{
    public class PlayerSpawnerConfigure : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [SerializeField] private Vector2 _size;

        #endregion

        #region Private properties

        private Vector3 _Position => transform.position;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var playerSpawner = new PlayerSpawner(_Position, _size);
            dstManager.AddComponentData(entity, playerSpawner);
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_Position, _size);
        }

        #endregion
    }
}