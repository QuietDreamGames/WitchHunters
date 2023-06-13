using Features.FiniteStateMachine;
using UnityEngine;

namespace Features.Character.States
{
    public class MeleeComboEntryState : MeleeBaseState
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            attackIndex = 1;
            CharacterView.PlayAttackAnimation(attackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                if (ShouldCombo)
                {
                    StateMachine.ChangeNextState(new MeleeComboState());
                }
                else
                {
                    StateMachine.ChangeNextStateToMain();
                }
            }
        }
    }
}