using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Enemies.Pools
{
    public class UnitPrewarmInstaller : ServiceInstaller
    {
        public override void Install()
        { 
            var unitPool = ServiceLocator.Resolve<GameObjectPool<UnitBehaviour>>();
            if (unitPool == null)
            {
                Debug.LogError("Unit pool is not registered", this);
                return;
            }
            
            var unitConfigurator = ServiceLocator.Resolve<UnitConfigurator>();
            if (unitConfigurator == null)
            {
                Debug.LogError("Unit configurator is not registered", this);
                return;
            }
            
            for (var i = 0; i < unitConfigurator.units.Length; i++)
            {
                var unitData = unitConfigurator.units[i];
                unitPool.Prewarm(unitData.unit.gameObject, unitData.count);
            }
        }
    }
}
