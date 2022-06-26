using Features.Character.Components;
using Features.HealthSystem.Components;
using Features.InputSystem.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Test.Services
{
    public class TestConverter :  MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Test Input Configuration")] 
        [SerializeField] private string _applyDamageActionID = "ApplyDamage";

        [Header("Test Input Wrapper")]
        [SerializeField] private PlayerInput _testInput;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var testInputWrapper = new TestInputWrapper { Value = _testInput };
            dstManager.AddSharedComponentData(entity, testInputWrapper);

            var testInputConfiguration = new TestInputConfiguration
            {
                ApplyDamageActionID = _applyDamageActionID,
            };
            dstManager.AddSharedComponentData(entity, testInputConfiguration);
        }

        #endregion
    }
}