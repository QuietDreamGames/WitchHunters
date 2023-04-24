using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Features.Trigger.Service
{
    public class InteractableTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _hint;
        [SerializeField] private string _sceneName;
        [SerializeField] private string _sceneAdditiveName;
         
        private bool _isInRange;

        public void OnInteract()
        {
            if (_isInRange)
            {
                SceneManager.LoadScene(_sceneName);
                SceneManager.LoadScene(_sceneAdditiveName, LoadSceneMode.Additive);
                // World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>().Enabled = false;
                // World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>().Enabled = false;
                //
                // StartCoroutine(StartPhysics());
            }

            _isInRange = false;
        }

        private IEnumerator StartPhysics()
        {
            yield return new WaitForSeconds(0.1f);
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>().Enabled = true;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>().Enabled = true;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            _hint.SetActive(true);
            _isInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _hint.SetActive(false);
            _isInRange = false;
        }
    }
}