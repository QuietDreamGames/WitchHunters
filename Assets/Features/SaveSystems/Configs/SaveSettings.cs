using System;
using UnityEditor;
using UnityEngine;

namespace Features.SaveSystems.Configs
{
    public class SaveSettings : ScriptableObject
    {
        [SerializeField] private SaveCategory[] saveCategories = {
            new()
            {
                category = "Settings",
                indexed = false,
                useUser = false,
                extension = ".data"
            }
        };
        
        public SaveCategory[] SaveCategories => saveCategories;

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
        
        [Serializable]
        public class SaveCategory
        {
            public string category;
            public bool indexed;
            public bool useUser;
            
            public string extension = ".data";
        }
    }
}