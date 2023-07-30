using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;

namespace Features.Character.States.Base
{
    public class UltimateSkillState : SkillState
    {
        public UltimateSkillState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();

            CharacterView.PlayUltimateAnimation();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsUltimateAnimationComplete())
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }
    }
}