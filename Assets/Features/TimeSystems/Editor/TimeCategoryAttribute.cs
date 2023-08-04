using Features.TimeSystems.Configs;
using UnityEngine;

namespace Features.TimeSystems.Editor
{
    public class TimeCategoryAttribute : PropertyAttribute
    {
        #if UNITY_EDITOR
        public readonly string[] timeCategories;
        
        public TimeCategoryAttribute()
        {
            timeCategories = TimeCategorySettings.GetSettings().TimeCategories;
        }
        #endif
    }
}