using Features.Activator.Core;
using Features.SaveSystems.Samples.Feature;
using UnityEngine;

namespace Features.Activator
{
    public class SaverActivator : MonoBehaviour, IActivator
    {
        [SerializeField] private SampleSaver saver;
        
        public void Activate()
        {
            saver.Save();
        }

        public void Deactivate()
        {
            
        }
    }
}
