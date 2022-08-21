using System;
using Features.Localization.Services;
using Features.Utils;
using UnityEditor;
using UnityEngine;

namespace Features.Localization
{
    public class LocalizationTool : MonoBehaviour
    {
        #region Singleton

        private static LocalizationTool _instance;

        public static LocalizationTool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<LocalizationTool>();
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject(typeof(LocalizationTool).Name);
                        _instance = singleton.AddComponent<LocalizationTool>();
                    }
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as LocalizationTool;
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
        
        public LocalizationData LocalizationData { get; private set; }

        public static Action OnLocalizationChanged;

        private void LoadLocalization(LocalizationLang localizationLang)
        {
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