using Features.Character.States;
using UnityEngine;
using UnityEngine.Serialization;

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

            var comboMovementState = new MeleeComboMovementState(_stateMachine, _secondComboAttackDistance, _secondComboAttackStartPercentage, _secondComboAttackEndPercentage);

            _stateMachine.AddState("IdleCombatState", new IdleCombatState(_stateMachine));
            _stateMachine.AddState("MoveState", new MoveState(_stateMachine));
            _stateMachine.AddState("MeleeEntryState", new MeleeEntryState(_stateMachine));
            _stateMachine.AddState("MeleeComboEntryState", new MeleeComboEntryState(_stateMachine));
            _stateMachine.AddState("MeleeComboState", comboMovementState);
            _stateMachine.AddState("MeleeFinisherState", new MeleeFinisherState(_stateMachine));
            
            _stateMachine.ChangeState("IdleCombatState");
        }
    }
}