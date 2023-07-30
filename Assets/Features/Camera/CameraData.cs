using Cinemachine;
using UnityEngine;

namespace Features.Camera
{
    public class CameraData : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        public void SetTarget(Transform target)
        {
            virtualCamera.ForceCameraPosition(target.position, Quaternion.identity);
            virtualCamera.Follow = target;
        }
        
    }
}
