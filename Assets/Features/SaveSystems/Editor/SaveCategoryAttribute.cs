using Features.SaveSystems.Configs;
using UnityEngine;

namespace Features.SaveSystems.Editor
{
    public class SaveCategoryAttribute : PropertyAttribute
    {
        public readonly SaveSettings.SaveCategory[] saveCategories;
        
        public SaveCategoryAttribute()
        {
            saveCategories = SaveSettings.GetSettings().SaveCategories;
        }
    }
}