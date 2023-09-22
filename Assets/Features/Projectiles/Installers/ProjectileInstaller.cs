using System;
using Features.Enemies.Extensions;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Projectiles.Installers
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
            
            var damageableObstaclePool = new GameObjectPool<DamageableObstacle>(transform);
            ServiceLocator.Register<GameObjectPool<DamageableObstacle>>(damageableObstaclePool);
        }
        
        [Serializable]
        private struct Data
        {
            public GameObject prefab;
            public int prewarmCount;
        }
    }
}
