using UnityEngine;

namespace Features.ServiceLocators.Core.Service
{
    public class ServiceContext : MonoBehaviour
    {
        [SerializeField] private ServiceInstaller[] installers;
        
        private void Awake()
        {
            for (var i = 0; i < installers.Length; i++)
            {
                installers[i].Install();
            }
        }
    }
}