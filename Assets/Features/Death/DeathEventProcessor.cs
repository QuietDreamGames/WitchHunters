using System;
using Features.Damage.Core;
using Features.Floor;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Death
{
    public class DeathEventProcessor : MonoBehaviour
    {
        public void SubscribeCharacterDeathEvent(DamageController damageController)
        {
            damageController.OnDeath += OnCharacterDeath;
        }

        public void UnsubscribeCharacterDeathEvent(DamageController damageController)
        {
            damageController.OnDeath -= OnCharacterDeath;
        }
        
        private void OnCharacterDeath()
        {
            // ServiceLocator.Resolve<FloorController>().RestartFloor();
        }
    }
}