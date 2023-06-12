using Features.FiniteStateMachine;
using UnityEngine;

namespace Features.Character.States
{
    public class MeleeEntryState : MeleeBaseState
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            State nextState = new MeleeComboEntryState();
            StateMachine.ChangeNextState(nextState);
        }
    }
}