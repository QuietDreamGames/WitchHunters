using System;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private Projectiles.Projectile projectilePrefab;
        
        [SerializeField] private Transform projectileSpawnPoint;

        private GameObjectPool<Projectiles.Projectile> _projectilePool;

        private void Start()
        {
            _projectilePool = ServiceLocator.Resolve<GameObjectPool<Projectiles.Projectile>>();
        }

        public Projectiles.Projectile Get()
        {
            var projectile = _projectilePool.Spawn(projectilePrefab.gameObject, null);
            projectile.Pool = _projectilePool;
            projectile.Prefab = projectilePrefab.gameObject;
            
            projectile.transform.position = projectileSpawnPoint.position;

            return projectile;
        }
    }
}
