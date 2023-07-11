using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Features.Structs
{
    [ExecuteInEditMode]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField] private string uniqueID;
        
        [SerializeField] private bool useCustomID;
        
        public string ID => uniqueID;
        
        #if UNITY_EDITOR

        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            var emptyID = string.IsNullOrEmpty(uniqueID);
            
            var currentStage = StageUtility.GetCurrentStage();
            if (currentStage is PreviewSceneStage)
            {
                var stageHandle = currentStage.stageHandle;
                if (stageHandle.Contains(gameObject))
                {
                    if (useCustomID == false)
                    {
                        if (emptyID == false)
                        {
                            uniqueID = null;
                            EditorUtility.SetDirty(this);
                        }
                    }
                }
                
                return;
            }

            if (emptyID)
            {
                GenerateID();
            }
        }

        private void GenerateID()
        {
            var go = gameObject;
            uniqueID = $"{go.scene.name}_{go.name}_{Guid.NewGuid()}";
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        #endif
    }
}