using Features.CameraShakes.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.CameraShakes
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private CameraShakeData[] data;

        private IShakeDirector _shakeDirector;
        
        private void Awake()
        {
            _shakeDirector = ServiceLocator.Resolve<IShakeDirector>();
        }
        
        [ContextMenu("Shake")]
        public void Shake()
        {
            data[0].RegisterShakeData(_shakeDirector);
        }
        
        [ContextMenu("Unshake")]
        public void Unshake()
        {
            data[0].UnregisterShakeData(_shakeDirector);
        }
        
        public void RegisterShake(int index)
        {
            data[index].RegisterShakeData(_shakeDirector);
        }
        
        public void UnregisterShake(int index)
        {
            data[index].UnregisterShakeData(_shakeDirector);
        }
    }
}
