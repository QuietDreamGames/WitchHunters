using System.Collections.Generic;
using UnityEngine;

namespace Features.Localization.Services
{
    public class LocalizationData : ScriptableObject
    {
        public LocalizationLang LocalizationLang;
        public List<LineData> LinesData;

        public string GetLineByKey(string key)
        {
            for (int i = 0; i < LinesData.Count; i++)
            {
                if (LinesData[i].Key == key)
                {
                    return LinesData[i].Value;
                }
            }

            return "";
        }

        public bool ContainsKey(string key)
        {
            for (int i = 0; i < LinesData.Count; i++)
            {
                if (LinesData[i].Key == key)
                {
                    return true;
                }
            }

            return false;
        }
    }
    
    [System.Serializable]
    public class LineData {
        public string Key;
        public string Value;
    }
}