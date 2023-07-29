using UnityEngine;

namespace Features.ServiceLocators.Core.Service
{
    public class FacadeInstaller : ServiceInstaller
    {
        [SerializeField] private ServiceInstaller[] installers;

        public override void Install()
        {
            for (var i = 0; i < installers.Length; i++)
            {
                installers[i].Install();
            }
        }
    }
}
