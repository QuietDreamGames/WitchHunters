using System;
using UnityEngine;

namespace Features.Activator
{
    public class TriggerZone : MonoBehaviour
    {
        [Header("Interact Layer")]
        [SerializeField] private LayerMask triggerLayer;
        
        [Header("Dependencies")]
        [SerializeField] private GroupActivator groupActivator;
        
        [Header("Data")]
        [SerializeField] private Data data;
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (data.wasTriggered)
            {
                return;
            }
            
            var otherLayer = 1 << other.gameObject.layer;
            if ((otherLayer & triggerLayer) == 0)
            {
                return;
            }

            groupActivator.Activate();
            data.wasTriggered = true;
        }

        [Serializable]
        private class Data
        {
            public bool wasTriggered;
        }
    }
}
