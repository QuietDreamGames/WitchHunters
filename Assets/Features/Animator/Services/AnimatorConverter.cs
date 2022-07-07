using Features.Animator.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Animator.Services
{
    public class AnimatorConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Animator Configuration")] 
        [SerializeField] private string _movingParam = "Move";

        [SerializeField] private string _horizontalParam = "Horizontal";
        [SerializeField] private string _verticalParam = "Vertical";
        
        [SerializeField] private string _attackParam = "Attack";
        
        [Header("Animator")] 
        [SerializeField] private UnityEngine.Animator _animator;

        #endregion
        
        #region IConvertGameObjectToEntity implementation
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var animator = new AnimatorWrapper { Value = _animator };
            dstManager.AddSharedComponentData(entity, animator);

            var configuration = new AnimatorConfiguration
            {
                Moving = _movingParam,
                
                Horizontal = _horizontalParam,
                Vertical = _verticalParam,
                
                Attack = _attackParam
            };
            dstManager.AddComponentData(entity, configuration);
        }
        
        #endregion
    }
}