using System;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Character
{
    public class CharacterView : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        private static readonly int Magnitude = Animator.StringToHash("Magnitude");
        private static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
        private static readonly int LastVertical = Animator.StringToHash("LastVertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Ultimate = Animator.StringToHash("Ultimate");
        private static readonly int Secondary = Animator.StringToHash("Secondary");

        private static readonly int Shield = Animator.StringToHash("Shield");

        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Reset = Animator.StringToHash("Reset");
        // private static readonly int AttackColliderActive = Animator.StringToHash("AttackCollider.Active");
        // private static readonly int AttackComboWindowOpen = Animator.StringToHash("AttackComboWindow.Open");


        public void PlayIdleAnimation(Vector2 lastMovementDirection)
        {
            _animator.SetFloat(Magnitude, 0);

            if (MathF.Abs(lastMovementDirection.x) - MathF.Abs(lastMovementDirection.y) > -0.001f)
            {
                _animator.SetFloat(LastHorizontal, Mathf.Round(lastMovementDirection.x));
                _animator.SetFloat(LastVertical, 0);
            }
            else
            {
                _animator.SetFloat(LastHorizontal, 0);
                _animator.SetFloat(LastVertical, Mathf.Round(lastMovementDirection.y));
            }
        }
        
        public void PlayWalkAnimation(Vector2 movementDirection)
        {
            _animator.SetFloat(Magnitude, movementDirection.magnitude);
            _animator.SetFloat(Horizontal, movementDirection.x);
            _animator.SetFloat(Vertical, movementDirection.y);
        }
        
        public void PlayAttackAnimation(int index)
        {
            _animator.SetTrigger("Attack" + index);
        }
        
        public bool IsAttackAnimationJustTriggered(int attackIndex)
        {
            return _animator.GetBool("Attack" + attackIndex);
        }
        
        public bool IsAttackAnimationComplete(int attackIndex)
        {
            bool isAnimCompl = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
            bool isAnimName = _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackIndex);
            bool isJustTriggered = _animator.GetBool("Attack" + attackIndex);
            // return isAnimCompl && isAnimName && !isJustTriggered;

            if (isJustTriggered)
                return false;
            if (!isAnimName)
                return true;
            return isAnimCompl;
        }
        
        public float CurrentAnimationTimeNormalized()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        public float CurrentAnimationTimeSum()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).length * _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        public Vector3 GetLastMovementDirection()
        {
            return new Vector3(_animator.GetFloat(LastHorizontal), _animator.GetFloat(LastVertical), 0);
        }
        
        public float GetCurrentAnimationLength()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).length;
        }
        
        public bool IsUltimateAnimationComplete()
        {
            bool isAnimCompl = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
            bool isJustTriggered = _animator.GetBool(Ultimate);

            return !isJustTriggered && isAnimCompl;
        }
        
        public bool IsSecondaryAnimationComplete()
        {
            bool isAnimCompl = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
            bool isJustTriggered = _animator.GetBool(Secondary);

            return !isJustTriggered && isAnimCompl;
        }
        
        public void PlayUltimateAnimation()
        {
            _animator.SetTrigger(Ultimate);
        }
        
        public void PlaySecondaryAnimation()
        {
            _animator.SetTrigger(Secondary);
        }
        
        public void PlayDeathAnimation()
        {
            _animator.SetTrigger(Death);
        }
        
        public void ResetTrigger()
        {
            _animator.SetTrigger(Reset);
        }

        public void SetShieldAnimation(bool isShieldActive)
        {
            _animator.SetBool(Shield, isShieldActive);
        }

        // public bool IsAttackColliderActive()
        // {
        //     return _animator.GetFloat(AttackColliderActive) > 0.5f;
        // }
        //
        // public bool IsAttackComboWindowOpen()
        // {
        //     return _animator.GetFloat(AttackComboWindowOpen) > 0.5f;
        // }
        public void OnUpdate(float deltaTime)
        {
            _animator.Update(deltaTime);
        }
    }
}