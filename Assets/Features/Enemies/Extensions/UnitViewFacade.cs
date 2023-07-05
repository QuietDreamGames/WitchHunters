using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class UnitViewFacade : MonoBehaviour
    {
        [SerializeField] private UnitView unitView;
        
        public void CompleteAnimation()
        {
            unitView.CompleteAnimation();
        }
    }
}
