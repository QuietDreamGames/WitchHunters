using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using TextAsset = UnityEngine.TextCore.Text.TextAsset;

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

        [MenuItem("Localization/Download Data")]
        public static void LoadCSV()
        {
            EditorCoroutineUtility.StartCoroutine(DownloadData(OnLoad), Instance);
        }


        private static void OnLoad(string str)
        {
            Debug.Log(str);
        }



        internal static IEnumerator DownloadData(System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();
 
            string downloadData = null;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                Debug.Log("Starting Download...");
                yield return webRequest.SendWebRequest();
                int equalsIndex = ExtractEqualsIndex(webRequest.downloadHandler);
                if (webRequest.isNetworkError || (-1 == equalsIndex))
                {
                    Debug.Log("...Download Error: " + webRequest.error);
                    // downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                    // string versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
                    // Debug.Log("Using stale data version: " + versionText);
                }
                else
                {
                    string versionText = webRequest.downloadHandler.text.Substring(0, equalsIndex);
                    downloadData = webRequest.downloadHandler.text.Substring(equalsIndex + 1);
                    // PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
                    // PlayerPrefs.SetString("LastDataDownloaded", downloadData);
                    Debug.Log("...Downloaded version: " + versionText);
 
                }
            }
 
            onCompleted(downloadData);
        }
        
        private static int ExtractEqualsIndex(DownloadHandler d)
        {
            if (d.text == null || d.text.Length < 10)
            {
                return -1;
            }
            // First term will be proceeded by version number, e.g. "100=English"
            string versionSection = d.text.Substring(0, 5);
            int equalsIndex = versionSection.IndexOf('=');
            if (equalsIndex == -1)
                Debug.Log("Could not find a '=' at the start of the CVS");
            return equalsIndex;
        }
    }

    // public class LocalizationFileInfo
    // {
    //     public LocalizationLang LocalizationLanguage;
    //     public string LocalizationFilePath;
    // }
}