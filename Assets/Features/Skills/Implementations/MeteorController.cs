using Features.Damage.Core;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.ObjectPools.Core;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class MeteorController : MonoBehaviour, IUpdateHandler, IFixedUpdateHandler
    {
        [SerializeField] private Vector3 _startDirection;
        [SerializeField] private float _distance;
        [SerializeField] private float _timeBeforeHit;
        [SerializeField] private float _timeAfterHit;

        [SerializeField] private Animator _meteorAnimator;
        [SerializeField] private Animator _explosionAnimator;
        [SerializeField] private Animator _ashAnimator;
        
        [SerializeField] private Collider2D _hitboxCollider;
        
        private Transform _meteorTransform;

        private bool _isUpdating;
        private bool _isFalling;
        private float _timer;
        
        public GameObjectPool<MeteorController> Pool { get; set; } = null;
        public GameObject Prefab {get; set; } = null;
        
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        private AColliderDamageProcessor _damageProcessor;
        
        private static readonly int Start = Animator.StringToHash("Start");
        private static readonly int Stop = Animator.StringToHash("Stop");

        public void Cast(ModifiersContainer modifiersContainer,
            BaseModifiersContainer baseModifiersContainer, AColliderDamageProcessor damageProcessor)
        {
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            _damageProcessor = damageProcessor;
            _damageProcessor.SetCollider(_hitboxCollider);
            
            _meteorTransform = _meteorAnimator.transform;
            _meteorTransform.position = transform.position + _startDirection.normalized * _distance;
            _meteorTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_startDirection.y, _startDirection.x) * Mathf.Rad2Deg - 90);
            _isUpdating = true;
            _isFalling = true;
            _timer = _timeBeforeHit;
            _meteorAnimator.SetTrigger(Start);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isUpdating) return;
            
            _meteorAnimator.Update(deltaTime);
            _explosionAnimator.Update(deltaTime);
            _ashAnimator.Update(deltaTime);
            
            _timer -= Time.deltaTime;
            switch (_timer)
            {
                case <= 0 when _isFalling:
                    _isFalling = false;
                    _timer = _timeAfterHit;
                    _meteorAnimator.SetTrigger(Stop);
                    _explosionAnimator.SetTrigger(Start);
                    _ashAnimator.SetTrigger(Start);
                    _damageProcessor.InstantProcessDamage();
                    break;
                case <= 0:
                    _isUpdating = false;
                    Pool.Despawn(Prefab, this);
                    break;
            }
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!_isUpdating) return;
            _meteorTransform.position = Vector3.MoveTowards(_meteorTransform.position, transform.position,
                _distance / _timeBeforeHit * Time.fixedDeltaTime);
        }
    }
}