using Cinemachine;
using UnityEngine;

namespace Features.Camera.Services
{
    public class CameraConfigure : MonoBehaviour
    {
        #region Serializable data

        [Header("Camera Configure")] 
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _cameraFollowTarget;
        
        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _virtualCamera.Follow = _cameraFollowTarget;
            
            Destroy(this);
        }

        #endregion
    }
}
