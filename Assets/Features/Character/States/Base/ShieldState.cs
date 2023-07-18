using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using Features.VFX;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class ShieldState : State
    {
        private ShieldEffectController _shieldEffectController;
        private ShieldHealthController _shieldHealthController;
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        private CharacterView _characterView;
        private PlayerInput _playerInput;
        
        public ShieldState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _shieldEffectController = stateMachine.GetExtension<ShieldEffectController>();
            _modifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            _baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
            _characterView = stateMachine.GetExtension<CharacterView>();
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _shieldHealthController = stateMachine.GetExtension<ShieldHealthController>();
            
            _characterView.SetShieldAnimation(true);
            _shieldEffectController.PlayShieldEffect();
            _shieldHealthController.SetShieldActive(true);
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerInput.actions["Shield"].IsPressed() == false)
            {
                _characterView.SetShieldAnimation(false);
                _shieldEffectController.StopShieldEffect();
                stateMachine.ChangeState("IdleCombatState");
                _shieldHealthController.SetShieldActive(false);
                return;
            }
            
            _shieldHealthController.GetShieldHealth(out var shieldCurrentHealth, out var shieldMaxHealth);
            
            if (shieldCurrentHealth <= 0)
            {
                _characterView.SetShieldAnimation(false);
                _shieldEffectController.PlayShieldDestroyEffect();
                stateMachine.ChangeState("IdleCombatState");
                _shieldHealthController.SetShieldActive(false);
                return;
            }
        }

        public override void OnFixedUpdate(float deltaTime)
        {
        }

        public override void OnLateUpdate(float deltaTime)
        {
        }
    }
}