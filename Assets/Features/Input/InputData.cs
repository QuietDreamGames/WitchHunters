using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Features.Input
{
    public class InputData : MonoBehaviour
    {
        public PlayerInput playerInput;
        public EventSystem eventSystem;
        public InputSystemUIInputModule inputSystemUi;
    }
}
