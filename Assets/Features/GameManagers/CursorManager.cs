using System;
using UnityEngine;

namespace Features.GameManagers
{
    public class CursorManager : MonoBehaviour
    {
        private bool _visible = true;
        
        public void SetVisible(bool visible)
        {
            if (_visible == visible)
            {
                return;
            }
            
            _visible = visible;
            UpdateVisibility();
        }
        
        private void UpdateVisibility()
        {
            Cursor.visible = _visible;
            if (_visible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                UpdateVisibility();
            }
        }
    }
}
