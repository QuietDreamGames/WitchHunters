using UnityEngine;

namespace Features.UI.TabSystem
{
    public abstract class TabContent : MonoBehaviour
    {
        [SerializeField] private TabType _tabType;
        [SerializeField] protected GameObject _tabContent;
        public TabType TabType => _tabType;

        public virtual void OnSelect()
        {
            _tabContent.SetActive(true);
        }

        public virtual void OnDeselect()
        {
            _tabContent.SetActive(false);
        }
    }
}