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
            ServiceLocator.Register<TimeSystem>(timeSystem);
        }
    }
}