using Features.SaveSystems.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Test
{
    public class SavesClear : MonoBehaviour
    {
        public void ClearSaves()
        {
            var saves = ServiceLocator.Resolve<SaveSystem>();
            saves.Clear("Gameplay");
            saves.LoadFromDisk("Gameplay");
            saves.Load("Gameplay");

        }
    }
}