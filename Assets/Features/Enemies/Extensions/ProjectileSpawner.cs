using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private Projectiles.Projectile projectilePrefab;
        
        [SerializeField] private Transform projectileSpawnPoint;

        public Projectiles.Projectile Get()
        {
            return Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        }
    }
}
