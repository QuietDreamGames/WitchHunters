using Features.VFX;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 forceDirection = default, HitEffectType hitEffectType = default);
    }
}