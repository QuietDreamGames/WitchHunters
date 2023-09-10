using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using Features.TimeSystems.Core;
using UnityEngine;

namespace Features.TimeSystems.Installer
{
    public class TimeSystemInstaller : ServiceInstaller
    {
        [SerializeField] private TimeSystem timeSystem;
        
        public override void Install()
        {
            var exist = ServiceLocator.Resolve<TimeSystem>();
            if (exist != null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ServiceLocator.Register<TimeSystem>(timeSystem);
            
            timeSystem.transform.parent = null;
            DontDestroyOnLoad(timeSystem);
        }
    }
}