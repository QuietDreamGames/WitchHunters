using Features.SaveSystems.Configs;
using UnityEngine;

namespace Features.SaveSystems.Editor
{
    public class SaveCategoryAttribute : PropertyAttribute
    {
        #if UNITY_EDITOR
        public readonly SaveCategory[] saveCategories;
        
        public SaveCategoryAttribute()
        {
            saveCategories = SaveSettings.GetSettings().SaveCategories;
        }
        #endif
    }
}