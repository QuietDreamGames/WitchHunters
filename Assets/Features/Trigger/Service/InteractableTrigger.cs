using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Trigger.Service
{
    public class InteractableTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _hint;
        [SerializeField] private string _sceneName;
        private bool _isInRange;
        

        private void Update()
        {
            if (_isInRange && Input.GetKeyDown("E"))
            {
                SceneManager.LoadScene(_sceneName);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _hint.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _hint.SetActive(false);
        }
    }
}