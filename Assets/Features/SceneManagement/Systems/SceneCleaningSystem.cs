using Features.SceneManagement.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Features.SceneManagement.Systems
{
    public partial class SceneCleaningSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // _queryDesc = new EntityQueryDesc
            // {
            //     None = new ComponentType[] { typeof(SceneId) }
            // };
        }
        
        protected override void OnDestroy()
        {
            base.OnCreate();
            SceneManager.sceneLoaded -= OnSceneLoaded;

        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);

            if (mode == LoadSceneMode.Single)
            {
                Entities.WithNone<DontDestroyOnLoadFlag>().ForEach((Entity e) =>
                {
                    EntityManager.DestroyEntity(e);
                }).WithStructuralChanges().Run();
            }
            
        }
        
        protected override void OnUpdate()
        {
            
        }
    }
}