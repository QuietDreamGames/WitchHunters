using System.Collections.Generic;
using UnityEngine;

namespace Features.Localization.Services
{
    public class LocalizationData : ScriptableObject
    {
        public LocalizationLang LocalizationLang;
        public Dictionary<string, string> Lines;
    }
}