using System.Collections;
using System.Collections.Generic;
using Features.Projectile;
using Features.Projectile.Core;
using UnityEngine;

namespace Features.ColliderController.Core
{
    public class RangeColliderController : MonoBehaviour
    {
        [SerializeField] private AProjectile[] _collidersPool;
        
        public void BurstColliders(ProjectileAnimationEventInfo projectileAnimationEventInfo)
        {
            var projectiles = new List<AProjectile>();

            for (int i = 0; i < _collidersPool.Length; i++)
            {
                if (projectileAnimationEventInfo.rangeColliderType != _collidersPool[i].rangeColliderInfo.rangeColliderType) continue;
                if (_collidersPool[i].isUsed) continue;
                
                int neededNumber = _collidersPool[i].rangeColliderInfo.baseBurstTimes  * _collidersPool[i].rangeColliderInfo.baseAmountInBurst;

                _collidersPool[i].isUsed = true;
                projectiles.Add(_collidersPool[i]);
                
                if (projectiles.Count == neededNumber) break;
            }
            
            if (projectiles.Count == 0) return;

            StartCoroutine(BurstRoutine(projectiles, projectiles[0].rangeColliderInfo.baseAmountInBurst,
                projectiles[0].rangeColliderInfo.baseBurstInterval,
                transform.position + projectileAnimationEventInfo.offset, projectileAnimationEventInfo.direction));
        }

        private IEnumerator BurstRoutine(List<AProjectile> projectiles, int numberInBurst, float interval, Vector3 position, Vector2 facingDirection)
        {
            float facingRotation = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            float angleInterval = projectiles[0].rangeColliderInfo.additionalAngle;
            float projectileSpread = angleInterval * (numberInBurst - 1f);
            float startRotation = facingRotation + projectileSpread / 2f;
            
            var projCounter = 0;
            for (int i = 0; i < projectiles.Count / numberInBurst; i++)
            {
                for (int j = 0; j < numberInBurst; j++)
                {
                    float tempRot = startRotation - angleInterval * j;
                    Vector3 tempDir = new Vector3(Mathf.Cos(tempRot * Mathf.Deg2Rad), Mathf.Sin(tempRot * Mathf.Deg2Rad), 0f);
                    
                    projectiles[projCounter].Fire(position, tempDir, tempRot);
                    projCounter++;
                }

                yield return new WaitForSeconds(interval);
            }
        }
    }
}