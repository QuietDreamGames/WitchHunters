using Features.FiniteStateMachine.Interfaces;
using UnityEngine;

namespace Features.Character.States.Base
{
    public class MeleeComboMovementState : MeleeBaseState
    {
        private float _distance;
        private float _speed;
        private Vector3 _attackDirection;
        private float _startPercentage; // at which point of animation should we start moving
        private float _endPercentage;
        private bool _shouldBeMoving;
        private Transform _transform;

        public MeleeComboMovementState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public MeleeComboMovementState(IMachine stateMachine, float distance, float startPercentage, float endPercentage) : base(stateMachine)
        {
            _distance = distance;
            _startPercentage = startPercentage;
            _endPercentage = endPercentage;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            attackIndex = 2;
            CharacterView.PlayAttackAnimation(attackIndex, attackSpeed);
            _transform = stateMachine.GetExtension<Transform>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                if (ShouldCombo)
                {
                    stateMachine.ChangeState("MeleeFinisherState");
                }
                else
                {
                    stateMachine.ChangeState("IdleCombatState");
                }
                _speed = 0;
                return;
            }
            
            if (CharacterView.IsAttackAnimationJustTriggered(attackIndex))
            {
                _shouldBeMoving = false;
                _speed = 0;
                return;
            }
            
            if (CharacterView.CurrentAnimationTimeNormalized() > _startPercentage && CharacterView.CurrentAnimationTimeNormalized() < _endPercentage)
            {
                _shouldBeMoving = true;
                _attackDirection = CharacterView.GetLastMovementDirection();
                
                var animLength = CharacterView.GetCurrentAnimationLength();
                _speed = _distance / (animLength * (_endPercentage - _startPercentage));
            }
            else
            {
                _shouldBeMoving = false;
            }
        }
        
        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            
            if (!_shouldBeMoving) return;
            
            var direction = _attackDirection * (_speed * deltaTime);
            _transform.position += direction;
        }
    }
}