using UnityEngine;

namespace Features.Damage.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 forceDirection = default);
    }
}