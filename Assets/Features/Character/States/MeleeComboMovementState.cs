using Features.FiniteStateMachine.Interfaces;
using UnityEngine;

namespace Features.Character.States
{
    public class MeleeComboMovementState : MeleeBaseState
    {
        private float _speed;
        private Vector2 _attackDirection;
        private float _startPercentage; // at which point of animation should we start moving
        private float _endPercentage;
        private bool _shouldBeMoving;
        private Transform _transform;

        public MeleeComboMovementState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public MeleeComboMovementState(IMachine stateMachine, float speed, float startPercentage, float endPercentage) : base(stateMachine)
        {
            _speed = speed;
            _startPercentage = startPercentage;
            _endPercentage = endPercentage;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            attackIndex = 2;
            CharacterView.PlayAttackAnimation(attackIndex);
            _transform = stateMachine.GetExtension<Transform>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.CurrentAttackAnimationTime() > _startPercentage && CharacterView.CurrentAttackAnimationTime() < _endPercentage)
            {
                _shouldBeMoving = true;
                _attackDirection = CharacterView.GetLastMovementDirection();
            }
            else
            {
                _shouldBeMoving = false;
            }
            
            
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                stateMachine.ChangeState("MeleeFinisherState");
            }
        }
        
        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            
            if (!_shouldBeMoving) return;
            
            var direction = _attackDirection * (_speed * deltaTime);
            _transform.position += new Vector3(direction.x, direction.y, 0);
        }
    }
}