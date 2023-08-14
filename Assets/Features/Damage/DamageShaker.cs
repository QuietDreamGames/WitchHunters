using Features.CameraShakes.Core;
using Features.Damage.Core;
using Features.ServiceLocators.Core;
using Features.VFX.Core;
using UnityEngine;

namespace Features.Damage
{
    public class DamageShaker : MonoBehaviour
    {
        [SerializeField] private CameraShakeData data;
        
        [Header("Dependencies")]
        [SerializeField] private DamageController damageController;
        
        private IShakeDirector _shakeDirector;
        
        private void Awake()
        {
            _shakeDirector = ServiceLocator.Resolve<IShakeDirector>();
        }
        
        private void OnEnable()
        {
            damageController.OnAnyHit += OnAnyHitHandler;
        }

        private void OnDisable()
        {
            damageController.OnAnyHit -= OnAnyHitHandler;
        }
        
        
        private void OnAnyHitHandler(Vector3 vector, HitEffectType hitType)
        {
            data.RegisterShakeData(_shakeDirector);
        }
    }
}