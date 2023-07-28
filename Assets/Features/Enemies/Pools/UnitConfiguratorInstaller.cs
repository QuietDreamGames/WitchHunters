using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Enemies.Pools
{
    public class UnitConfiguratorInstaller : ServiceInstaller
    {
        [SerializeField] private UnitConfigurator unitConfigurator;
        
        public override void Install()
        {
            ServiceLocator.Register<UnitConfigurator>(unitConfigurator);
            unitConfigurator.Construct();
        }
    }
}
