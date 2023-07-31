using Cinemachine;
using UnityEngine;

namespace Features.Camera
{
    public class CameraData : MonoBehaviour
    {
        [SerializeField] private new UnityEngine.Camera camera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        public void SetTarget(Transform target)
        {
            virtualCamera.Follow = target;
            virtualCamera.ForceCameraPosition(target.position, Quaternion.identity);
        }
        
    }
}
