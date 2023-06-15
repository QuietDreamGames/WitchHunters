using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using UnityEngine;

namespace Features.Character.States
{
    public class MeleeComboEntryState : MeleeBaseState
    {
        

        public MeleeComboEntryState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            attackIndex = 1;
            CharacterView.PlayAttackAnimation(attackIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                if (ShouldCombo)
                {
                    stateMachine.ChangeState("MeleeComboState");
                }
                else
                {
                    stateMachine.ChangeState("IdleCombatState");
                }
            }
        }
    }
}