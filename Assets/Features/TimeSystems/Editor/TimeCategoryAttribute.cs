using Features.TimeSystems.Configs;
using UnityEngine;

namespace Features.TimeSystems.Editor
{
    public class TimeCategoryAttribute : PropertyAttribute
    {
        public readonly string[] timeCategories;
        
        public TimeCategoryAttribute()
        {
            timeCategories = TimeCategorySettings.GetSettings().TimeCategories;
        }
    }
}