using Features.FiniteStateMachine;
using UnityEngine;

namespace Features.Character.States
{
    public class MeleeComboState : MeleeBaseState
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            attackIndex = 2;
            CharacterView.PlayAttackAnimation(attackIndex);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (CharacterView.IsAnimationComplete())
            {
                if (ShouldCombo)
                {
                    StateMachine.ChangeNextState(new MeleeFinisherState());
                }
                else
                {
                    StateMachine.ChangeNextStateToMain();
                }
            }
        }
    }
}