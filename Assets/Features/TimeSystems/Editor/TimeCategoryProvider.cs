using Features.TimeSystems.Configs;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace Features.TimeSystems.Editor
{
    #if UNITY_EDITOR
    internal static class TimeCategoryProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateTimeCategoryProvider()
        {
            var provider = new SettingsProvider("Tools/TimeCategorySettings", SettingsScope.User)
            {
                label = "Time Categories",
                activateHandler = (searchContext, rootElement) =>
                {
                    var settings = TimeCategorySettings.GetSettings();
                    var serializedSettings = new SerializedObject(settings);
                    
                    var title = new Label("Time Categories");
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
    #endif
}