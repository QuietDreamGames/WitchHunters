using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Network;
using Features.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class IdleCombatState : State
    {
        private PlayerInput _playerInput;
        private NetworkInput _networkInput;
        private SkillsController _skillsController;
        private BaseModifiersContainer _baseModifiersContainer;
        private ShieldHealthController _shieldHealthController;
        
        public IdleCombatState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _playerInput = stateMachine.GetExtension<PlayerInput>();
            _networkInput = stateMachine.GetExtension<NetworkInput>();
            _skillsController = stateMachine.GetExtension<SkillsController>();
            _baseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
            _shieldHealthController = stateMachine.GetExtension<ShieldHealthController>();
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            var inputData = _networkInput.InputData;
            if (inputData.attack)
            {
                stateMachine.ChangeState("MeleeEntryState");
                return;
            }
            
            if (inputData.secondary)
            {
                
                if (!_skillsController.Secondary.IsOnCooldown) stateMachine.ChangeState("SecondarySkillState");
                return;
            }
            
            if (inputData.ultimate)
            {
                
                if (!_skillsController.Ultimate.IsOnCooldown) stateMachine.ChangeState("UltimateSkillState");
                return;
            }
            
            if (inputData.shield)
            {
                _shieldHealthController.GetShieldHealth(out var shieldCurrentHealth, out var shieldMaxHealth);
                if (shieldCurrentHealth > 0)
                {
                    stateMachine.ChangeState("ShieldState");
                    return;
                }
            }

            if (inputData.move != Vector2.zero)
            {
                stateMachine.ChangeState("MoveState");
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