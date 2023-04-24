using System;
using Unity.Entities;
using UnityEngine;

namespace Features.UI.DeathScreen.Services
{
    public class DeathScreenConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private UnityEngine.Animator _animator;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var deathScreen = new Components.DeathScreen()
            {
                Animator =  _animator,
                IsRunning = false
            };
            dstManager.AddComponentData(entity, deathScreen);
        }
    }
}