using System;
using FMODUnity;
using UnityEngine;

namespace Features.Sounds
{
    [Serializable]
    public class SoundData
    {
        public EventReference @event;

        public bool isAttached;
        
        //private 
        
        public void StartSound(GameObject go)
        {
            if (isAttached)
            {
                RuntimeManager.PlayOneShotAttached(@event, go);
                var instance = RuntimeManager.CreateInstance(@event);
            }
            else
            {
                RuntimeManager.PlayOneShot(@event, go.transform.position);
            }
        }
    }
}
