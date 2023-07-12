using System;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Projectiles
{
    public class ProjectileInstaller : ServiceInstaller
    {
        [SerializeField] private Data[] data;

        public override void Install()
        {
            var projectilePool = new GameObjectPool<Projectile>(transform);
            ServiceLocator.Register<GameObjectPool<Projectile>>(projectilePool);
            
            for (var i = 0; i < data.Length; i++)
            {
                var projectileData = data[i];
                projectilePool.Prewarm(projectileData.prefab, projectileData.prewarmCount);
            }
        }
        
        [Serializable]
        private struct Data
        {
            public GameObject prefab;
            public int prewarmCount;
        }
    }
}
