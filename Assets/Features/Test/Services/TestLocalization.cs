using System.Collections;
using System.Collections.Generic;
using Features.Localization;
using Features.Localization.Services;
using Unity.Entities;
using UnityEngine;

public class TestLocalization : MonoBehaviour
{
    public void OnLanguageChangeToRussian()
    {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<LocalizationSystem>().ChangeLocalization(LocalizationLang.Russian);
    }
    
    public void OnLanguageChangeToEnglish()
    {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<LocalizationSystem>().ChangeLocalization(LocalizationLang.English);
    }
}
