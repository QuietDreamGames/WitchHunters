using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Features.Localization;
using Features.Localization.Services;
using TMPro;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    public string RawText;
    public string Text { get; private set; }

    private TextMeshProUGUI _textComponent;
    
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
            
            var textToReplace = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<LocalizationSystem>()
                .GetLocalizationLine(searchKey);
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
        
        LocalizationSystem.OnLocalizationChanged += OnLocalizationChange;
        UpdateText(RawText);
    }

    private void OnDisable()
    {
        LocalizationSystem.OnLocalizationChanged -= OnLocalizationChange;
    }
}
