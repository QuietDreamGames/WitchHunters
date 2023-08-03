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
            var unitPool = new GameObjectPool<UnitBehaviour>(transform);
            ServiceLocator.Register(unitPool);
        }
    }
}
