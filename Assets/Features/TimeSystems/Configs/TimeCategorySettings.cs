#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Features.TimeSystems.Configs
{
    #if UNITY_EDITOR
    internal class TimeCategorySettings : ScriptableObject
    {
        [SerializeField] private string[] timeCategories;
        
        internal string[] TimeCategories => timeCategories;

        internal static TimeCategorySettings GetSettings()
        {
            var guids = AssetDatabase.FindAssets("t:TimeCategorySettings");
            if (guids.Length > 1)
            {
                Debug.LogWarning($"Found multiple settings files, using the first.");
            }

            switch (guids.Length)
            {
                case 0:
                    return null;
                default:
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    return AssetDatabase.LoadAssetAtPath<TimeCategorySettings>(path);
            }
        }
    }
    #endif
}