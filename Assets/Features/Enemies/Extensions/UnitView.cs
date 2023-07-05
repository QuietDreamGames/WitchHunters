using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Enemies.Extensions
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private Transform view;
        
        [SerializeField] private Renderer renderer;
        [SerializeField] private Animator animator;

        [SerializeField] private UnitConfig config;

        private int facingDirection;

        public Action OnCompleteAnimation;

        private void Start()
        {
            SetFacingDirection(Random.value);
        }

        public void SetFacingDirection(float direction)
        {
            if (direction != 0)
            {
                var directionSign = direction > 0 ? 1 : -1;
                if (directionSign != facingDirection)
                {
                    facingDirection = directionSign;
                    
                    var scale = view.localScale;
                    scale.x = math.abs(scale.x) * facingDirection;
                    view.localScale = scale;
                }
            }
        }

        public void SetVelocity(Vector2 direction)
        {
            var horizontal = direction.x;
            var vertical = direction.y;

            SetFacingDirection(horizontal);
            
            animator.SetBool(config.HorizontalMoveParam, horizontal != 0);
            animator.SetBool(config.VerticalMoveParam, vertical != 0);
        }
        
        public void SetAttack(int attackID)
        {
            animator.SetInteger(config.AttackIDParam, attackID);
            animator.SetTrigger(config.AttackParam);
        }
        
        public void CompleteAnimation()
        {
            OnCompleteAnimation?.Invoke();
            OnCompleteAnimation = null;
        }
    }
}
