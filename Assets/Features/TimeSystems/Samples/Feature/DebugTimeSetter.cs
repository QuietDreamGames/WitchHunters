using Features.ServiceLocators.Core;
using Features.TimeSystems.Core;
using Features.TimeSystems.Editor;
using UnityEngine;

namespace Features.TimeSystems.Samples.Feature
{
    public class DebugTimeSetter : MonoBehaviour
    {
        #region Seralizable fileds

        [SerializeField][TimeCategory] private string category;
        [SerializeField] private float timeScale;

        #endregion
        
        private TimeSystem _timeSystem;

        private void Start()
        {
            _timeSystem = ServiceLocator.Resolve<TimeSystem>();
        }

        [ContextMenu("Set time scale")]
        public void SetTimeScale()
        {
            _timeSystem.SetCategoryTimeScale(category, timeScale);
        }
    }
}
