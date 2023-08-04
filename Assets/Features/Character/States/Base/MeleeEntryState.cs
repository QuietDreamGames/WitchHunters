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
            var comboController = stateMachine.GetExtension<ComboController>();
            var attackNr = comboController.GetAttackComboNr() + 1;
            stateMachine.ChangeState($"MeleeCombo{attackNr}State");
        }
    }
}