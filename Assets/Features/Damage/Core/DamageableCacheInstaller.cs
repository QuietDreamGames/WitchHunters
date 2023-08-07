using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageableCacheInstaller : ServiceInstaller
    {
        [SerializeField] private DamageableCache damageableCache;
        
        public override void Install()
        {
            ServiceLocator.Register<DamageableCache>(damageableCache);
        }
    }
}
