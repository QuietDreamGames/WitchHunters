using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.SaveSystems.Configs
{
    public class SaveSettings : ScriptableObject
    {
        [SerializeField] private SaveCategory[] saveCategories = {
            new()
            {
                category = "Settings",
                useUser = false,
                useSaveSlot = false,
                useCharacter = false,
                extension = ".data"
            }
        };
        
        public SaveCategory[] SaveCategories => saveCategories;
        
        #if UNITY_EDITOR

        public static SaveSettings GetSettings()
        {
            var guids = AssetDatabase.FindAssets("t:SaveSettings");
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
                    return AssetDatabase.LoadAssetAtPath<SaveSettings>(path);
            }
        }
        #endif
    }
    
    [Serializable]
    public class SaveCategory
    {
        public string category;
        public bool useUser;
        public bool useSaveSlot;
        public bool useCharacter;
        
        public string extension = ".data";
    }
}