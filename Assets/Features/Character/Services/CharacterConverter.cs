using Features.Character.Components;
using Features.HealthSystem.Components;
using Features.InputSystem.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.Services
{
    public class CharacterConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Health Data")]
        [SerializeField] private float _maxHealth = 100f;

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

            var health = new Health
            {
                InitialValue = _maxHealth,
                Value = _maxHealth
            };
            dstManager.AddComponentData(entity, health);
            
            var speed = new Speed { Value = _speed };
            dstManager.AddComponentData(entity, speed);

            var movement = new Movement
            {
                Direction = new float2(1, 0), 
                Enable = false
            };
            dstManager.AddComponentData(entity, movement);
        }

        #endregion
    }
}
