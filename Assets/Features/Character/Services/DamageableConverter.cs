using Features.Character.Components;
using Features.HealthSystem.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class DamageableConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data

        [Header("Health Data")]
        [SerializeField] private float _maxHealth = 100f;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var health = new Health(_maxHealth);
            dstManager.AddComponentData(entity, health);

            var damageTag = new DamageableTag();
            dstManager.AddComponentData(entity, damageTag);

            dstManager.AddBuffer<Damage>(entity);
        }

        #endregion
    }
}
