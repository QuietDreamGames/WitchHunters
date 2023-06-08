using System;
using Features.Localization.Services;
using Features.Utils;
using UnityEngine;

namespace Features.Localization
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        public LocalizationData LocalizationData { get; private set; }

        public static Action OnLocalizationChanged;

        private void LoadLocalization(LocalizationLang localizationLang)
        {
            var assetName = @"Localizations/" + localizationLang.ToString() + @"Localization";

            var result = Resources.Load<LocalizationData>(assetName);

            if (result != null)
            {
                LocalizationData = result;
            }
            else
            {
                Debug.LogError(
                    $"Error! Didn't load the localization data by the asset name = {assetName}");
            }
            
            // string[] result = AssetDatabase.FindAssets(assetName);
            
            // if (result.Length == 1)
            // {
            //     var assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
            //     LocalizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(assetPath);
            // }
            // else
            // {
            //     Debug.LogError($"Error! {result.Length} localization files of {localizationLang} language were found!" +
            //                    $"It should be equal to 1!");
            // }
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
    }
}