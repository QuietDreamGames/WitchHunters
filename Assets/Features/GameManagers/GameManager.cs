using System;
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
        [SerializeField] private string mainMenuSceneName;
        
        [Space]
        [SerializeField] private string masterSceneName;
        
        [Space]
        [SerializeField] private string hubSceneName;
        [SerializeField] private string dungeonSceneName;
        
        [Header("Time Categories")]
        [SerializeField, TimeCategory] private string gameplayTimeCategory;
        
        private readonly List<Scene> _activeScenes = new(4);
        
        private LoadingScreenUI _loadingScreenUI;
        private GameStateManager _gameStateManager;
        
        public Action OnSceneChange;

        private void Awake()
        {
            GetDependencies();
        }
        
        public void Restart()
        {
            StartHub();
        }
        
        public void StartMainMenu()
        {
            _gameStateManager.SetGameState(GameStates.Menu);
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }

        public void StartMasterScene()
        {
            var isLoaded = IsSceneLoaded(masterSceneName);
            if (!isLoaded)
            {
                _gameStateManager.SetGameState(GameStates.Menu);
                SceneManager.LoadScene(masterSceneName, LoadSceneMode.Single);
            }
        }
        
        public void StartHub()
        {
            StartGameplayScene(hubSceneName);
        }
        
        public void StartDungeon()
        {
            StartGameplayScene(dungeonSceneName);
        }
        
        public void StartGameplayScene(string gameplaySceneName)
        {
            StartMasterScene();
            ChangeScene(gameplaySceneName);
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
            yield return null;

            GetDependencies();
            
            _loadingScreenUI.Reset();
            yield return _loadingScreenUI.SetAlphaCoroutine(1);
            
            _gameStateManager.SetGameState(GameStates.Menu);
            
            OnSceneChange?.Invoke();
            
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

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            
            _gameStateManager.SetGameState(GameStates.Gameplay);
            
            _loadingScreenUI.Reset();
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

        private void GetDependencies()
        {
            if (_loadingScreenUI == null)
            {
                _loadingScreenUI = ServiceLocator.Resolve<LoadingScreenUI>();
            }
            if (_gameStateManager == null)
            {
                _gameStateManager = ServiceLocator.Resolve<GameStateManager>();
            }
        }
    }
}
