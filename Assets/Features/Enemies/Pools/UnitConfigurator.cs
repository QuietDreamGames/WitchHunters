using System;
using UnityEngine;

namespace Features.Enemies.Pools
{
    [CreateAssetMenu(fileName = "UnitConfigurator", menuName = "Unit/UnitConfigurator")]
    public class UnitConfigurator : ScriptableObject
    {
        public UnitConfiguratorData[] units;
        
        [Serializable]
        public class UnitConfiguratorData
        {
            public UnitBehaviour unit;
            public int count;
        }
    }
}
