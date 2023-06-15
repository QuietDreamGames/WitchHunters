using UnityEngine;

namespace Features.Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        private static readonly int Magnitude = Animator.StringToHash("Magnitude");
        private static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
        private static readonly int LastVertical = Animator.StringToHash("LastVertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        // private static readonly int AttackColliderActive = Animator.StringToHash("AttackCollider.Active");
        // private static readonly int AttackComboWindowOpen = Animator.StringToHash("AttackComboWindow.Open");


        public void PlayIdleAnimation(Vector2 lastMovementDirection)
        {
            _animator.SetFloat(Magnitude, 0);
            _animator.SetFloat(LastHorizontal, lastMovementDirection.x);
            _animator.SetFloat(LastVertical, lastMovementDirection.y);
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
        
        public bool IsAttackAnimationComplete(int attackIndex)
        {
            bool isAnimCompl = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
            // bool isAnimName = _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackIndex);
            bool isJustTriggered = _animator.GetBool("Attack" + attackIndex);

            // Debug.Log("isAnimCompl: " + isAnimCompl + " isAnimName: " + isAnimName + " isJustTriggered: " + isJustTriggered);
            
            // return isAnimCompl && isAnimName && !isJustTriggered;
            return isAnimCompl && !isJustTriggered;
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
    }
}