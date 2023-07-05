using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States.Base
{
    public class MeleeComboState : MeleeBaseState
    {
        public MeleeComboState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            attackIndex = 2;
            CharacterView.PlayAttackAnimation(attackIndex);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                if (ShouldCombo)
                {
                    stateMachine.ChangeState("MeleeFinisherState");
                }
                else
                {
                    stateMachine.ChangeState("IdleCombatState");
                }
            }
        }
    }
}