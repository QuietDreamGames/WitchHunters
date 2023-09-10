using UnityEngine;

namespace Features.Projectiles
{
    public class FollowProjectile : Projectile
    {
        private Vector3 _direction;
        
        protected override void OnSpawn()
        {
            _direction = (Target.position - transform.position).normalized;
            
            RotateView(_direction);
        }

        protected override void OnDespawn()
        {
            
        }

        protected override void Translate(float deltaTime)
        {
            _direction = (Target.position - transform.position).normalized;
            
            RotateView(_direction);
            
            transform.Translate(_direction * (Speed * deltaTime));
        }
    }
}
