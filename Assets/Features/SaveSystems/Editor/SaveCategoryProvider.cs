using Features.SaveSystems.Configs;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Features.SaveSystems.Editor
{
    internal static class SaveCategoryProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSaveCategoryProvider()
        {
            var provider = new SettingsProvider("Tools/SaveCategorySettings", SettingsScope.User)
            {
                label = "Save Categories",
                activateHandler = (searchContext, rootElement) =>
                {
                    var settings = SaveSettings.GetSettings();
                    var serializedSettings = new SerializedObject(settings);
                    
                    var title = new Label("Save Categories");
                    title.AddToClassList("title");
                    rootElement.Add(title);
                    
                    var properties = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Column
                        }
                    };
                    properties.AddToClassList("property-list");
                    rootElement.Add(properties);
                    
                    properties.Add(new InspectorElement(serializedSettings));
                    
                    rootElement.Bind(serializedSettings);
                }
            };
            return provider;
        }
    }
}