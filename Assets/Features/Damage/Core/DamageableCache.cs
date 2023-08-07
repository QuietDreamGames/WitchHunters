using System.Collections.Generic;
using Features.Damage.Interfaces;
using UnityEngine;

namespace Features.Damage.Core
{
    public class DamageableCache : MonoBehaviour
    {
        private Dictionary<Transform, IDamageable> _damageables = new();

        public void SubscribeDamageable(Transform origin, IDamageable damageable)
        {
            _damageables[origin] = damageable;
        }
        
        public void UnsubscribeDamageable(Transform origin)
        {
            var contains = _damageables.ContainsKey(origin);
            if (!contains)
            {
                return;
            }
            
            _damageables.Remove(origin);
        }
        
        public IDamageable GetDamageable(Transform origin)
        {
            var contains = _damageables.ContainsKey(origin);
            if (!contains)
            {
                return null;
            }

            return _damageables[origin];
        }
    }
}
