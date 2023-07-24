using Features.Health;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Interfaces;
using Features.VFX;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Core
{
    public class CharacterDamageController : DamageController
    {
        [SerializeField] private ShieldEffectController _shieldEffectController;
        
        private IShieldHealthController _shieldHealthController;
        
        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent, IShieldHealthController shieldHealthController)
        {
            _shieldHealthController = shieldHealthController;
            base.Initiate(modifiersController, baseModifiersContainer, healthComponent);
        }
        
        public override void TakeDamage(float damage, Vector3 forceDirection, HitEffectType hitEffectType)
        {
            var damageAfterShield = _shieldHealthController.GetHit(damage);
            if (damageAfterShield <= 0)
            {
                _shieldEffectController.PlayShieldHitEffect();
            }

            base.TakeDamage(damageAfterShield, forceDirection, hitEffectType);
        }
    }
}