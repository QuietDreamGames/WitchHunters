using System;
using Features.Localization.Services;
using Features.Utils;
using Unity.Entities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Features.Localization
{
    public partial class LocalizationSystem : SystemBase
    {
        public LocalizationData LocalizationData { get; private set; }

        public static Action OnLocalizationChanged;

        private void LoadLocalization(LocalizationLang localizationLang)
        {
            #if UNITY_EDITOR
            var assetName = @"" + localizationLang.ToString() + @"Localization";
            string[] result = AssetDatabase.FindAssets(assetName);
            
            if (result.Length == 1)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
                LocalizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(assetPath);
            }
            else
            {
                Debug.LogError($"Error! {result.Length} localization files of {localizationLang} language were found!" +
                               $"It should be equal to 1!");
            }
            #endif
        }

        #region Public methods

        public string GetLocalizationLine(string key)
        {
            var localizationLine = "<BAD DATA>";

            if (LocalizationData == null)
            {
                LoadLocalization((LocalizationLang)PlayerPrefs.GetInt(SaveKeys.CurrentLocalization, 0));
            }

            if (LocalizationData.ContainsKey(key))
                localizationLine = LocalizationData.GetLineByKey(key);

            return localizationLine;
        }

        public void ChangeLocalization(LocalizationLang localizationLang)
        {
            PlayerPrefs.SetInt(SaveKeys.CurrentLocalization, (int)localizationLang);
            LoadLocalization(localizationLang);
            OnLocalizationChanged?.Invoke();
        }
        
        #endregion

        protected override void OnUpdate()
        {
            
        }
    }
}   