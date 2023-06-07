using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Features.Localization.Services
{
    public class LocalizationLoader
    {
        //private List<LocalizationFileInfo> _localizationFileInfos;
        // private static string[] _lineSeparator = { Environment.NewLine };
        // private char _surround = '"';
        // private string[] _fieldsSeparator = { "\",\"" };
        
        private const string k_googleSheetDocID = "10wxy1KGzGsrPDwo-3mADF9ucpYzjGd-VfBVP5CFj_Bg";
 
        // docs.google.com/spreadsheets/d/13zXZxMWmS5ShIIxXd8OIOIf6JCBYmwziav9OsLdPH1U/edit#gid=0
        private const string url = "https://docs.google.com/spreadsheets/d/" + k_googleSheetDocID + "/export?format=csv";

        #region singleton
        private LocalizationLoader() {}  
        private static LocalizationLoader instance = null;  
        private static LocalizationLoader Instance {  
            get {  
                if (instance == null) {  
                    instance = new LocalizationLoader();
                    
                }  
                return instance;  
            }  
        }
        #endregion
        
        private static List<LocalizationData> _uninitiatedLocalizationDatas;

        [MenuItem("Localization/Download Data")]
        public static void StartDownloadData()
        {
            EditorCoroutineUtility.StartCoroutine(DownloadData(OnDownload), Instance);
        }
        
        private static void OnDownload(string str)
        {
            //Debug.Log(str);
            EditorCoroutineUtility.StartCoroutine(ProcessAndSaveData(str), Instance);
        }

        private static IEnumerator ProcessAndSaveData(string data)
        {
            // Line level
            int currLineIndex = 0;
            bool inQuote = false;
            int linesSinceUpdate = 0;
            int kLinesBetweenUpdate = 15;

            // Entry level
            string currEntry = "";
            int currCharIndex = 0;
            bool currEntryContainedQuote = false;
            List<string> currLineEntries = new List<string>();
            
            _uninitiatedLocalizationDatas = new List<LocalizationData>();
            
#if UNITY_EDITOR_OSX
            char lineEnding = '\r';
            int lineEndingLength = 2;
#else
            char lineEnding = '\n';
            int lineEndingLength = 1;
#endif
            
            while (currCharIndex < data.Length)
            {
                if (!inQuote && (data[currCharIndex] == lineEnding))
                {
                    // Skip the line ending
                    currCharIndex += lineEndingLength;

                    // Wrap up the last entry
                    // If we were in a quote, trim bordering quotation marks
                    if (currEntryContainedQuote)
                    {
                        currEntry = currEntry.Substring(1, currEntry.Length - 2);
                    }

                    currEntry = currEntry.Substring(0, currEntry.Length - 1);

                    currLineEntries.Add(currEntry);
                    currEntry = "";
                    currEntryContainedQuote = false;

                    // Line ended
                    ProcessLineFromCSV(currLineEntries, currLineIndex);
                    currLineIndex++;
                    currLineEntries = new List<string>();

                    linesSinceUpdate++;
                    if (linesSinceUpdate > kLinesBetweenUpdate)
                    {
                        linesSinceUpdate = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    if (data[currCharIndex] == '"')
                    {
                        inQuote = !inQuote;
                        currEntryContainedQuote = true;
                    }

                    // Entry level stuff
                    {
                        if (data[currCharIndex] == ',')
                        {
                            if (inQuote)
                            {
                                currEntry += data[currCharIndex];
                            }
                            else
                            {
                                // If we were in a quote, trim bordering quotation marks
                                if (currEntryContainedQuote)
                                {
                                    currEntry = currEntry.Substring(1, currEntry.Length - 2);
                                }

                                currLineEntries.Add(currEntry);
                                currEntry = "";
                                currEntryContainedQuote = false;
                            }
                        }
                        else
                        {
                            currEntry += data[currCharIndex];
                        }
                    }
                    currCharIndex++;
                }
            }

            foreach (var alreadyInitializedFile in _uninitiatedLocalizationDatas)
            {
                // Debug.Log(alreadyInitializedFile.LocalizationLang + " <<<<<<<<<<<<<");
                foreach (var lineData in alreadyInitializedFile.LinesData)
                {
                    Debug.Log($"{lineData.Key} => {lineData.Value}");
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
        {
            if (currLineIndex == 0)
            {
                // Skip first line 'cause it's just "Id"
                for (int columnIndex = 1; columnIndex < currLineElements.Count; columnIndex++)
                {
                    string currColumnName = currLineElements[columnIndex];
                    
                    foreach(LocalizationLang handledLang in Enum.GetValues(typeof(LocalizationLang)))
                    {
                        if (handledLang.ToString() == currColumnName)
                        {
                            CreateLocalizationFile(handledLang);
                        }
                    }
                }
                return;
            }
            
            var entryId = currLineElements[0];

            for (int i = 1; i < currLineElements.Count; i++)
            {
                _uninitiatedLocalizationDatas[i - 1].LinesData
                    .Add(new LineData { Key = entryId, Value = currLineElements[i] });
                EditorUtility.SetDirty(_uninitiatedLocalizationDatas[i - 1]);
            }
        }

        #region Creation of the asset file

        private static void CreateLocalizationFile(LocalizationLang localizationLang)
        {
            Debug.Log($"Creating localization file with {localizationLang} lang");
            
            var assetName = @"" + localizationLang.ToString() + @"Localization";
            var assetPath = @"Assets\Resources\Localizations\" + assetName + ".asset";

            string[] result = AssetDatabase.FindAssets(assetName);

            LocalizationData localizationData = null;
            
            if (result.Length > 1)
            {
                Debug.LogError($"More than 1 {assetName} Asset founded");
                return;
            }

            if (result.Length == 0)
            {
                localizationData = ScriptableObject.CreateInstance<LocalizationData>();
                
                AssetDatabase.CreateAsset(localizationData, assetPath);
                Debug.Log("Created new localization asset");
            }

            else
            {
                assetPath = AssetDatabase.GUIDToAssetPath(result[0]);
                localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(assetPath);
                Debug.Log("Found old localization asset, overriding it...");
            }
            
            localizationData.LocalizationLang = localizationLang;
            localizationData.LinesData = new List<LineData>();
            _uninitiatedLocalizationDatas.Add(localizationData);
            
            // EditorUtility.SetDirty(localizationData);
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
            // _uninitiatedLocalizationDatas.Add(new LocalizationData
            // {
            //     LocalizationLang = localizationLang, 
            //     
            // });
        }

        #endregion

        #region Downloading
        
        internal static IEnumerator DownloadData(System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();
 
            string downloadData = null;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();
                //int equalsIndex = ExtractEqualsIndex(webRequest.downloadHandler);
                if (webRequest.result == UnityWebRequest.Result.ConnectionError) // || (-1 == equalsIndex))
                {
                    Debug.Log("...Download Error: " + webRequest.error);
                    // downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                    // string versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
                    // Debug.Log("Using stale data version: " + versionText);
                }
                else
                {
                    //string versionText = webRequest.downloadHandler.text.Substring(0, equalsIndex);
                    downloadData = webRequest.downloadHandler.text;//.Substring(equalsIndex + 1);
                    // PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
                    // PlayerPrefs.SetString("LastDataDownloaded", downloadData);
                    Debug.Log("...Download finished"); // + versionText);
 
                }
            }
 
            onCompleted(downloadData);
        }
        
        // private static int ExtractEqualsIndex(DownloadHandler d)
        // {
        //     if (d.text == null || d.text.Length < 10)
        //     {
        //         return -1;
        //     }
        //     // First term will be proceeded by version number, e.g. "100=English"
        //     string versionSection = d.text.Substring(0, 5);
        //     int equalsIndex = versionSection.IndexOf('=');
        //     if (equalsIndex == -1)
        //         Debug.Log("Could not find a '=' at the start of the CVS");
        //     return equalsIndex;
        // }
        
        #endregion
    }
    
}