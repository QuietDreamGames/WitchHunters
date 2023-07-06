using Features.Team;
using UnityEngine;

namespace Features.Damage.Interfaces
{
    public interface IDamageable
    {
        TeamIndex TeamIndex { get; }
        void TakeDamage(float damage);
        void TakeDamage(float damage, Vector3 forceDirection);
    }
}