using UnityEngine;

namespace Features.Character
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        
        public void PlayIdleAnimation(Vector2 lastMovementDirection)
        {
            _animator.SetFloat("Magnitude", 0);
            _animator.SetFloat("LastHorizontal", lastMovementDirection.x);
            _animator.SetFloat("LastVertical", lastMovementDirection.y);
        }
        
        public void PlayWalkAnimation(Vector2 movementDirection)
        {
            _animator.SetFloat("Magnitude", movementDirection.magnitude);
            _animator.SetFloat("Horizontal", movementDirection.x);
            _animator.SetFloat("Vertical", movementDirection.y);
        }
        
        public void PlayAttackAnimation()
        {
            
        }
        
        
    }
}