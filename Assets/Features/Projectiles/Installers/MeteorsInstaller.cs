using System;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using Features.Skills.Implementations;
using UnityEngine;

namespace Features.Projectiles.Installers
{
    public class MeteorsInstaller : ServiceInstaller
    {
        [SerializeField] private Data[] _data;
        
        public override void Install()
        {
            var meteorsPool = new GameObjectPool<MeteorController>(transform);
            ServiceLocator.Register(meteorsPool);
            
            for (var i = 0; i < _data.Length; i++)
            {
                var meteorData = _data[i];
                meteorsPool.Prewarm(meteorData.prefab, meteorData.prewarmCount);
            }
        }
        
        [Serializable]
        private struct Data
        {
            public GameObject prefab;
            public int prewarmCount;
        }
    }
}