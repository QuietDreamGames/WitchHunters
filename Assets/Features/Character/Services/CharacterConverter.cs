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
    public class CharacterConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data
        
        [Header("Player Input Configuration")] 
        [SerializeField] private string _moveActionID = "Move";
        [SerializeField] private string _attackActionID = "Attack";

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // var playerInputWrapper = new PlayerInputWrapper { Value = _playerInput };
            // dstManager.AddSharedComponentData(entity, playerInputWrapper);

            var characterInput = new CharacterInput
            {
                Value = new InputInterpreter()
            };
            dstManager.AddComponentData(entity, characterInput);

            var inputConfiguration = new InputConfiguration
            {
                MoveActionID = _moveActionID,
                AttackActionID = _attackActionID
            };
            dstManager.AddSharedComponentData(entity, inputConfiguration);

            var autoAttackOverlapBox = new AttackOverlapBox();
            dstManager.AddComponentData(entity, autoAttackOverlapBox);
        }

        #endregion
    }
}
