using System;
using UnityEditor;
using UnityEngine;

namespace Features.TimeSystems.Editor
{
    [CustomPropertyDrawer(typeof(TimeCategoryAttribute))]
    internal class TimeCategoryDrawer : PropertyDrawer
    {
        public override void OnGUI(
            Rect position, 
            SerializedProperty property, 
            GUIContent label) 
        {
            var timeCategoryAttribute = attribute as TimeCategoryAttribute;
            if (timeCategoryAttribute == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var timeCategories = timeCategoryAttribute.timeCategories;
            var maxIndex = Array.IndexOf(timeCategories, property.stringValue);
            var index = Math.Max(0, maxIndex);
            index = EditorGUI.Popup(position, property.displayName, index, timeCategories);

            property.stringValue = timeCategories[index];
        }
    }
}