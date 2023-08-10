using UnityEngine;

namespace Features.Test
{
    public class AppSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 250;
            QualitySettings.vSyncCount = 0;
        }
    }
}