using Features.FiniteStateMachine;

namespace Features.Character.States
{
    public class MeleeFinisherState : MeleeBaseState
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            attackIndex = 3;
            CharacterView.PlayAttackAnimation(attackIndex);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (CharacterView.IsAnimationComplete())
            {
                StateMachine.ChangeNextStateToMain();
            }
        }
    }
}