using Features.Enemies.Extensions;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Enemies.Pools
{
    public class UnitInstaller : ServiceInstaller
    {
        public override void Install()
        {
            var unitPool = ServiceLocator.Resolve<GameObjectPool<UnitBehaviour>>();
            if (unitPool == null)
            {
                unitPool = new GameObjectPool<UnitBehaviour>(transform);
                ServiceLocator.Register(unitPool);
            }

            var unitConfigurator = ServiceLocator.Resolve<UnitConfigurator>();
            if (unitConfigurator != null)
            {
                for (var i = 0; i < unitConfigurator.units.Length; i++)
                {
                    var unitData = unitConfigurator.units[i];
                    unitPool.Prewarm(unitData.unit.gameObject, unitData.count);
                }
            }
        }
    }
}
