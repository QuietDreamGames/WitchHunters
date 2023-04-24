using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Gameplay
{
    public class DeathEventHandler : MonoBehaviour
    {
        public void LoadHubScene()
        {
            SceneManager.LoadScene("Hub");
        }
    }
}