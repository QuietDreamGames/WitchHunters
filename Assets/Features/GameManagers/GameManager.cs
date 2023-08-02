using System.Collections;
using System.Collections.Generic;
using Features.ServiceLocators.Core;
using Features.TimeSystems.Core;
using Features.TimeSystems.Editor;
using Features.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Features.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string masterSceneName;
        [SerializeField] private string dungeonSceneName;
        
        [Space]
        [SerializeField, TimeCategory] private string gameplayTimeCategory;
        
        private readonly List<Scene> _activeScenes = new(4);
        
        private LoadingScreenUI _loadingScreenUI;
        private TimeSystem _timeSystem;

        private void Awake()
        {
            _loadingScreenUI = ServiceLocator.Resolve<LoadingScreenUI>();
            _timeSystem = ServiceLocator.Resolve<TimeSystem>();
        }
        
        public void Restart()
        {
            StartMasterScene();
            
            StartDungeon();
        }

        public void StartMasterScene()
        {
            var isLoaded = IsSceneLoaded(masterSceneName);
            if (!isLoaded)
            {
                SceneManager.LoadScene(masterSceneName, LoadSceneMode.Single);
            }
        }
        
        public void StartDungeon()
        {
            ChangeScene(dungeonSceneName);
        }
        
        private bool IsSceneLoaded(string sceneName)
        {
            var countLoaded = SceneManager.sceneCount;
 
            for (int i = 0; i < countLoaded; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == sceneName)
                {
                    return true;
                }
            }

            return false;
        }

        private void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneRoutine(sceneName));
        }
        
        private IEnumerator ChangeSceneRoutine(string sceneName)
        {
            yield return _loadingScreenUI.SetAlphaCoroutine(1);
            
            _timeSystem.SetCategoryTimeScale(gameplayTimeCategory, 0);
            
            AsyncOperation async = null;
            
            GetActiveScenes(_activeScenes);
            for (var i = 0; i < _activeScenes.Count; i++)
            {
                var activeScene = _activeScenes[i];
                
                async = SceneManager.UnloadSceneAsync(activeScene);
                while (!async.isDone)
                {
                    yield return null;
                }
            }

            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!async.isDone)
            {
                yield return null;
            }
            
            _timeSystem.SetCategoryTimeScale(gameplayTimeCategory, 1);
            
            yield return _loadingScreenUI.SetAlphaCoroutine(0);
        }

        private void GetActiveScenes(ICollection<Scene> scenes)
        {
            scenes.Clear();
            
            var countLoaded = SceneManager.sceneCount;
            for (var i = 0; i < countLoaded; i++)
            {
                var loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.isLoaded)
                {
                    if (loadedScene.name == masterSceneName)
                    {
                        continue;
                    }
                    
                    scenes.Add(loadedScene);
                }
            }
        }
    }
}
