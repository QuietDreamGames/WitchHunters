using Features.Character.States.Base;
using Features.Character.States.Inquisitor;
using Features.Damage.Implementations;
using UnityEngine;

namespace Features.Character
{
    public class InquisitorCombatCharacterController : CombatCharacterController
    {
        [Header("SecondComboAttack")]
        [SerializeField] private float _secondComboAttackStartPercentage = 0.3f;
        [SerializeField] private float _secondComboAttackEndPercentage = 0.9f;
        [SerializeField] private float _secondComboAttackDistance = 0.5f;
        
        public override void Initiate()
        {
            base.Initiate();

            var comboMovementState = new InqMeleeCombo2State(stateMachine, _secondComboAttackDistance, _secondComboAttackStartPercentage, _secondComboAttackEndPercentage);

            stateMachine.AddState("IdleCombatState", new IdleCombatState(stateMachine));
            stateMachine.AddState("MoveState", new MoveState(stateMachine));
            stateMachine.AddState("MeleeEntryState", new MeleeEntryState(stateMachine));
            stateMachine.AddState("MeleeCombo1State", new InqMeleeCombo1State(stateMachine));
            stateMachine.AddState("MeleeCombo2State", comboMovementState);
            stateMachine.AddState("MeleeCombo3State", new InqMeleeCombo3State(stateMachine));
            stateMachine.AddState("SecondarySkillState", new SecondarySkillState(stateMachine));
            stateMachine.AddState("UltimateSkillState", new UltimateSkillState(stateMachine));
            stateMachine.AddState("ShieldState", new ShieldState(stateMachine));
            stateMachine.AddState("DeathState", new DeathState(stateMachine));
            
            stateMachine.ChangeState("IdleCombatState");
            
            var hittableLayerMask = LayerMask.GetMask($"Hittable", "Enemy");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");

            _meleeColliderController.Initiate(
                new InqSolarAttackDamageInstance(hittableLayerMask, obstacleLayerMask, modifiersContainer,
                    _baseModifiersContainer, _passiveController, transform),
                new InqSolarSecondaryMeleeDamageInstance(hittableLayerMask, obstacleLayerMask, modifiersContainer,
                        _baseModifiersContainer, _passiveController, transform)
                );
        }
    }
}