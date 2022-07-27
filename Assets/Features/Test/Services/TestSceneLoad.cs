using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Test.Services
{
    public class TestSceneLoad : MonoBehaviour
    {
        [SerializeField] private string _sceneName = "Scene";

        private bool _sceneState;

        [ContextMenu("Load Scene")]
        public void LoadScene()
        {
            var coroutine  = SceneChangeState(_sceneName, true);
            StartCoroutine(coroutine);
        }

        [ContextMenu("Unload Scene")]
        public void UnloadScene()
        {
            var coroutine  = SceneChangeState(_sceneName, false);
            StartCoroutine(coroutine);
        }

        private IEnumerator SceneChangeState(string sceneName, bool active)
        {
            if (active)
            {
                var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return load;
                Debug.Log("Scene - " + sceneName + " is loaded!");
            }
            else
            {
                var unload = SceneManager.UnloadSceneAsync(sceneName);
                yield return unload;
                Debug.Log("Scene - " + sceneName + " is unloaded!");
            }
        }
    }
}
