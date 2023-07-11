using UnityEngine;

namespace Features.VFX
{
    public class ShieldEffectController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem[] _destroyEffectParticleSystems;
        [SerializeField] private DestructiblePieceController[] _shieldPieces;
        
        private static readonly int IsActive = Animator.StringToHash("IsActive");
        private static readonly int ShieldDestroy = Animator.StringToHash("ShieldDestroy");
        private static readonly int ShieldHit = Animator.StringToHash("ShieldGotHit");

        public void Initiate()
        {
            for (int i = 0; i < _shieldPieces.Length; i++)
            {
                _shieldPieces[i].Initiate(transform);
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _shieldPieces.Length; i++)
            {
                _shieldPieces[i].OnUpdate(deltaTime);
            }
        }

        public void PlayShieldEffect()
        {
            _animator.SetBool(IsActive, true);
            
        }
        
        public void StopShieldEffect()
        {
            _animator.SetBool(IsActive, false);
        }
        
        public void PlayShieldDestroyEffect()
        {
            for (int i = 0; i < _destroyEffectParticleSystems.Length; i++)
            {
                _destroyEffectParticleSystems[i].Play();
            }
            
            for (int i = 0; i < _shieldPieces.Length; i++)
            {
                _shieldPieces[i].PlayDestructiblePieceEffect();
            }
            
            _animator.SetTrigger(ShieldDestroy);
            _animator.SetBool(IsActive, false);
        }

        public void PlayShieldHitEffect()
        {
            _animator.SetTrigger(ShieldHit);
        }
    }
}