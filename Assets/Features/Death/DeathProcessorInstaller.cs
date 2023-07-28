using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Death
{
    public class DeathProcessorInstaller : ServiceInstaller
    {
        [SerializeField] private DeathEventProcessor _deathProcessor;
        
        public override void Install()
        {
            ServiceLocator.Register(_deathProcessor);
        }
    }
}