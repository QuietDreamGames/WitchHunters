using System.Text.RegularExpressions;
using Features.ServiceLocators.Core;
using TMPro;
using UnityEngine;

namespace Features.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        public string RawText;
        private TextMeshProUGUI _textComponent;
    
        private LocalizationManager _localizationManager;
    
        public void UpdateText(string newRawText)
        {
            // if (!CheckStringFormat(newRawText))
            // {
            //     Debug.LogError($"{name}: Bad text format!");
            //     return;
            // }
        
            _textComponent.text = GetFormattedText(newRawText);
        }

        // private bool CheckStringFormat(string rawText)
        // {
        //     
        //     return true;
        // }

        private string GetFormattedText(string rawString)
        {
            var pattern = @"\[.*?\]";
            var matches = Regex.Matches(rawString, pattern, RegexOptions.IgnoreCase);

            foreach (var dataEntry in matches)
            {
                var searchKey = dataEntry.ToString();
                searchKey = searchKey[1..^1];

                var textToReplace = _localizationManager.GetLocalizationLine(searchKey);
                rawString = rawString.Replace(dataEntry.ToString(), textToReplace);
            }

            return rawString;
        }

        private void OnLocalizationChange()
        {
            UpdateText(RawText);
        }

        private void OnEnable()
        {
            _textComponent = GetComponent<TextMeshProUGUI>();
            _localizationManager = ServiceLocator.Resolve<LocalizationManager>();
        
            _localizationManager.OnLocalizationChanged += OnLocalizationChange;
            UpdateText(RawText);
        }

        private void OnDisable()
        {
            _localizationManager.OnLocalizationChanged -= OnLocalizationChange;
        }
    }
}