using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class DamageableObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private DamageableObstacle damageableObstaclePrefab;
        
        private GameObjectPool<DamageableObstacle> _damageableObstaclePool;

        private void Start()
        {
            _damageableObstaclePool = ServiceLocator.Resolve<GameObjectPool<DamageableObstacle>>();
        }

        public DamageableObstacle Get()
        {
            var damageableObstacle = _damageableObstaclePool.Spawn(damageableObstaclePrefab.gameObject, null);
            damageableObstacle.Pool = _damageableObstaclePool;
            damageableObstacle.Prefab = damageableObstaclePrefab.gameObject;
            
            return damageableObstacle;
        }
    }
}
