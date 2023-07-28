using Features.Death;
using Features.FiniteStateMachine;
using Features.Health;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ServiceLocators.Core;
using Features.Skills.Interfaces;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage.Core
{
    public class CharacterDamageController : DamageController
    {
        [SerializeField] private ShieldEffectController _shieldEffectController;
        
        private IShieldHealthController _shieldHealthController;
        private StateMachine _stateMachine;
        
        public void Initiate(ModifiersContainer modifiersController, BaseModifiersContainer baseModifiersContainer,
            HealthComponent healthComponent, StateMachine stateMachine, IShieldHealthController shieldHealthController)
        {
            base.Initiate(modifiersController, baseModifiersContainer, healthComponent);
            _shieldHealthController = shieldHealthController;
            _stateMachine = stateMachine;
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
        
        public override void Restart()
        {
            base.Restart();
            _stateMachine.ChangeState("IdleCombatState");
        }
        
        protected override void OnDeathEvent()
        {
            base.OnDeathEvent();
            _stateMachine.ChangeState("DeathState");
        }

        private void OnEnable()
        {
            ServiceLocator.Resolve<DeathEventProcessor>().SubscribeCharacterDeathEvent(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Resolve<DeathEventProcessor>().UnsubscribeCharacterDeathEvent(this);
        }
    }
}