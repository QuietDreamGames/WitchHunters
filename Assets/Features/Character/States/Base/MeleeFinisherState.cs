using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States.Base
{
    public class MeleeFinisherState : MeleeBaseState
    {
        public MeleeFinisherState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            attackIndex = 3;
            CharacterView.PlayAttackAnimation(attackIndex, attackSpeed);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }
    }
}