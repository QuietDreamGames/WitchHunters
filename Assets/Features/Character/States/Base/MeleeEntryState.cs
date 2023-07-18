using Features.FiniteStateMachine.Interfaces;
using Features.Skills.Core;

namespace Features.Character.States.Base
{
    public class MeleeEntryState : MeleeBaseState
    {
        public MeleeEntryState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            stateMachine.ChangeState("MeleeCombo1State");
        }
    }
}