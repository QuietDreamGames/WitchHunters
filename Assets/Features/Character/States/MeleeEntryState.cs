using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;

namespace Features.Character.States
{
    public class MeleeEntryState : MeleeBaseState
    {
        public MeleeEntryState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            stateMachine.ChangeState("MeleeComboEntryState");
        }
    }
}