using TMPro;
using Unity.Entities;
using UnityEngine.UI;

namespace Features.UI.CharacterHealth
{
    [GenerateAuthoringComponent]
    public class HUDHealthBarUIData : IComponentData
    {
        public Image Image;
        public TextMeshProUGUI Text;
    }
}