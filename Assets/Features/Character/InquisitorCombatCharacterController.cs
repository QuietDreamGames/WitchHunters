using Features.Character.States;
using UnityEngine;

namespace Features.Character
{
    public class InquisitorCombatCharacterController : CombatCharacterController
    {
        [Header("SecondComboAttack")]
        [SerializeField] private float _secondComboAttackStartPercentage = 0.3f;
        [SerializeField] private float _secondComboAttackEndPercentage = 0.9f;
        [SerializeField] private float _secondComboAttackDistance = 0.5f;
        
        protected override void Start()
        {
            base.Start();

            var comboMovementState = new MeleeComboMovementState(stateMachine, _secondComboAttackDistance, _secondComboAttackStartPercentage, _secondComboAttackEndPercentage);

            stateMachine.AddState("IdleCombatState", new IdleCombatState(stateMachine));
            stateMachine.AddState("MoveState", new MoveState(stateMachine));
            stateMachine.AddState("MeleeEntryState", new MeleeEntryState(stateMachine));
            stateMachine.AddState("MeleeComboEntryState", new MeleeComboEntryState(stateMachine));
            stateMachine.AddState("MeleeComboState", comboMovementState);
            stateMachine.AddState("MeleeFinisherState", new MeleeFinisherState(stateMachine));
            stateMachine.AddState("UltimateSkillState", new UltimateSkillState(stateMachine));
            
            stateMachine.ChangeState("IdleCombatState");
        }
    }
}