using System.Collections;
using Features.Character.States.Base;
using Features.FiniteStateMachine.Interfaces;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Character.States.Inquisitor
{
    public class InqMeleeCombo1State : MeleeBaseState
    {
        private APassiveController _passiveController;
        
        public InqMeleeCombo1State(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _passiveController = stateMachine.GetExtension<APassiveController>();
            
            var isCharged = _passiveController.CurrentPassiveInfo.IsCharged;
            
            attackIndex = isCharged ? 4 : 1;
            
            CharacterView.PlayAttackAnimation(attackIndex, attackSpeed);
            
            var comboController = stateMachine.GetExtension<ComboController>();
            comboController.OnAttack(CharacterView);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!CharacterView.IsAttackAnimationComplete(attackIndex)) return;
            
            if (ShouldCombo)
            {
                stateMachine.ChangeState("MeleeCombo2State");
            }
            else
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }
    }
}