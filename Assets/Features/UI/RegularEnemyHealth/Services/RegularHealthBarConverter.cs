using Features.UI.RegularEnemyHealth.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.UI.RegularEnemyHealth.Services
{
    public class RegularHealthBarConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private Transform _fullHealthBar;
        [SerializeField] private Transform _currentHealthBar;
        [SerializeField] private GameObject _healthBarParent;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            
            var healthBar = new RegularHealthBar
            {
                FullHealthBarTransform = _fullHealthBar,
                CurrentHealthBarTransform = _currentHealthBar,
                GameObject = _healthBarParent
            };
            dstManager.AddComponentData(entity, healthBar);
        }
    }
}