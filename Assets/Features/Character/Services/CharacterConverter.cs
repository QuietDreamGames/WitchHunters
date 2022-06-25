using Features.Character.Components;
using Features.InputSystem.Components;
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

        [Header("Collider")] 
        [SerializeField] private PhysicsShapeAuthoring _physicsShape;

        [Header("Speed Data")] 
        [SerializeField] private float _speed = 2f;

        [Header("Player Input Configuration")] 
        [SerializeField] private string _moveActionID = "Move";
        [SerializeField] private string _attackActionID = "Attack";
        
        [Header("Player Input Wrapper")]
        [SerializeField] private PlayerInput _playerInput;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var playerInputWrapper = new PlayerInputWrapper { Value = _playerInput };
            dstManager.AddSharedComponentData(entity, playerInputWrapper);

            var playerInputConfiguration = new PlayerInputConfiguration
            {
                MoveActionID = _moveActionID,
                AttackActionID = _attackActionID
            };
            dstManager.AddSharedComponentData(entity, playerInputConfiguration);
            
            var speed = new Speed { Value = _speed };
            dstManager.AddComponentData(entity, speed);

            var movement = new Movement
            {
                Direction = new float3(1, 0, 0),
                Enable = false
            };
            dstManager.AddComponentData(entity, movement);

            var physicsBox = _physicsShape.GetBoxProperties();
            var collider = new Collider
            {
                Size = physicsBox.Size
            };
            dstManager.AddComponentData(entity, collider);
        }

        #endregion
    }
}
