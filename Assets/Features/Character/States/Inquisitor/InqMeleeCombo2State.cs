﻿using Features.Character.States.Base;
using Features.FiniteStateMachine.Interfaces;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Character.States.Inquisitor
{
    public class InqMeleeCombo2State : MeleeBaseState
    {
        private APassiveController _passiveController;
        
        private float _distance;
        private float _speed;
        private Vector3 _attackDirection;
        private float _startPercentage; // at which point of animation should we start moving
        private float _endPercentage;
        private bool _shouldBeMoving;
        private Transform _transform;

        public InqMeleeCombo2State(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public InqMeleeCombo2State(IMachine stateMachine, float distance, float startPercentage, float endPercentage) : base(stateMachine)
        {
            _distance = distance;
            _startPercentage = startPercentage;
            _endPercentage = endPercentage;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            _passiveController = stateMachine.GetExtension<APassiveController>();
            var isCharged = _passiveController.CurrentPassiveInfo.IsCharged;
            attackIndex = isCharged ? 5 : 2;
            CharacterView.PlayAttackAnimation(attackIndex);
            
            _transform = stateMachine.GetExtension<Transform>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsAttackAnimationComplete(attackIndex))
            {
                if (ShouldCombo)
                {
                    stateMachine.ChangeState("MeleeCombo3State");
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