using System;
using UnityEditor;
using UnityEngine;

namespace Features.SaveSystems.Editor
{
    [CustomPropertyDrawer(typeof(SaveCategoryAttribute))]
    internal class SaveCategoryDrawer : PropertyDrawer
    {
        public override void OnGUI(
            Rect position, 
            SerializedProperty property, 
            GUIContent label) 
        {
            var saveCategoryAttribute = attribute as SaveCategoryAttribute;
            if (saveCategoryAttribute == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var saveCategories = saveCategoryAttribute.saveCategories;
            var categories = new string[saveCategories.Length];
            for (var i = 0; i < saveCategories.Length; i++)
            {
                categories[i] = saveCategories[i].category;
            }
            var maxIndex = Array.IndexOf(categories, property.stringValue);
            var index = Math.Max(0, maxIndex);
            index = EditorGUI.Popup(position, property.displayName, index, categories);

            property.stringValue = categories[index];
        }
    }
}