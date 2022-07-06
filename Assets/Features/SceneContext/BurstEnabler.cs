#if UNITY_EDITOR
using UnityEditor;

namespace Features.SceneContext
{
    public class BurstEnabler : Editor
    {
        [MenuItem("Utility/Burst Options/Enable Burst")]
        public static void EnableBurst()
        {
            EditorPrefs.SetBool("BurstCompilation", true);
        }
        
        [MenuItem("Utility/Burst Options/Disable Burst")]
        public static void DisableBurst()
        {
            EditorPrefs.SetBool("BurstCompilation", false);
        }
    }
}

#endif
