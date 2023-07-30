using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;

namespace Features.Character.States.Base
{
    public class SecondarySkillState : SkillState
    {
        public SecondarySkillState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            CharacterView.PlaySecondaryAnimation();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsSecondaryAnimationComplete())
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }
    }
}