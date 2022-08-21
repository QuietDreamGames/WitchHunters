using System.Collections;
using System.Collections.Generic;
using Features.Localization;
using Features.Localization.Services;
using UnityEngine;

public class TestLocalization : MonoBehaviour
{
    public void OnLanguageChangeToRussian()
    {
        LocalizationTool.Instance.ChangeLocalization(LocalizationLang.Russian);
    }
    
    public void OnLanguageChangeToEnglish()
    {
        LocalizationTool.Instance.ChangeLocalization(LocalizationLang.English);
    }
}
