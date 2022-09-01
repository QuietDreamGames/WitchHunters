using Features.Character.Components;
using Features.HealthSystem.Components;
using Features.InputSystem.Components;
using Features.InputSystem.Services;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;
using UnityEngine.InputSystem;
using Collider = Features.Collision.Components.Collider;

namespace Features.Character.Services
{
    public class PlayerConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data
        
        [Header("Player Input Configuration")] 
        [SerializeField] private string _moveActionID = "Move";
        [SerializeField] private string _attackActionID = "Attack";

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var characterInput = new CharacterInput();
            dstManager.AddComponentData(entity, characterInput);

            var inputConfiguration = new InputConfiguration(_moveActionID, _attackActionID);
            dstManager.AddComponentData(entity, inputConfiguration);

            var playerTag = new PlayerTag();
            dstManager.AddComponentData(entity, playerTag);
        }

        #endregion
    }
}
