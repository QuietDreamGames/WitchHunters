using UnityEngine;

namespace Features.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon;
        
        public ItemSortType sortType;
        
        public float weight;
    }
}