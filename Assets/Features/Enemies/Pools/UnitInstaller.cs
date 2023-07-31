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

            
        }
    }
}
