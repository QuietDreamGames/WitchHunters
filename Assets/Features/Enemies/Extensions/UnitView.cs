using System;
using System.Collections;
using Features.TimeSystems.Interfaces.Handlers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Enemies.Extensions
{
    public class UnitView : MonoBehaviour,IUpdateHandler
    {
        [Header("Face Parameters")]
        [SerializeField] private float faceDirectionTime = .125f;
        
        [Header("Dependencies")]
        [SerializeField] private Transform view;
        
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Animator animator;

        [SerializeField] private UnitConfig config;
        
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();
        
        private Coroutine _faceDirectionCoroutine;
        
        private int _facingDirection;
        
        private float _deltaTime;

        public Action OnCompleteAnimation;
        public Action<string> OnEvent; 

        private void Start()
        {
            SetFacingDirection(Random.value);
        }

        public void SetFacingDirection(float direction)
        {
            if (direction != 0)
            {
                var directionSign = direction > 0 ? 1 : -1;
                if (directionSign != _facingDirection)
                {
                    _facingDirection = directionSign;
                    
                    if (_faceDirectionCoroutine != null)
                    {
                        StopCoroutine(_faceDirectionCoroutine);
                        _faceDirectionCoroutine = null;
                    }
                    
                    _faceDirectionCoroutine = StartCoroutine(SetFacingDirectionCoroutine(directionSign));
                }
            }
        }
        
        public void SetMovementValueParams(float horizontal, float vertical, float dumpTime = 0.1f)
        {
            if (!config.UseMovementValueParam) 
                return;

            var instant = IsZero(dumpTime);
            
            if (!IsZero(horizontal))
            {
                if (instant)
                {
                    animator.SetFloat(config.HorizontalValueParam, horizontal);
                }
                else
                {
                    animator.SetFloat(config.HorizontalValueParam, horizontal, dumpTime, Time.deltaTime);
                }
            }
                
            if (!IsZero(vertical))
            {
                if (instant)
                {
                    animator.SetFloat(config.VerticalValueParam, vertical);
                }
                else
                {
                    animator.SetFloat(config.VerticalValueParam, vertical, dumpTime, Time.deltaTime);
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
            
            SetMovementValueParams(horizontal, vertical);
        }
        
        public void SetAttack(int attackID)
        {
            animator.SetInteger(config.AttackIDParam, attackID);
            animator.SetTrigger(config.AttackParam);
        }
        
        public void SetTrigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }
        
        public void CompleteAnimation()
        {
            OnCompleteAnimation?.Invoke();
            OnCompleteAnimation = null;
        }
        
        public void EventInvoke(string eventName)
        {
            OnEvent?.Invoke(eventName);
            OnEvent = null;
        }

        public void OnUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
        }
        
        private IEnumerator SetFacingDirectionCoroutine(int direction)
        {
            var currentTime = 0f;

            while (currentTime < faceDirectionTime)
            {
                currentTime += _deltaTime;

                yield return _waitForEndOfFrame;
            }
            
            var localScale = view.localScale;
            localScale.x = math.abs(localScale.x) * direction;
            view.localScale = localScale;
            
            _faceDirectionCoroutine = null;
        }
        
        private bool IsZero(float value)
        {
            return math.abs(value) < float.Epsilon;
        }
    }
}
