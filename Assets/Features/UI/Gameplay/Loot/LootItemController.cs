using Features.Inventory.Item;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.UI.Gameplay.Loot
{
    public class LootItemController : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _currencySprite;
        
        public InventoryItem item;
        public int currency;
        
        public void Initiate(InventoryItem item, int currency = 0)
        {
            this.item = item;
            this.currency = currency;
            
            if (item != null)
            {
                _icon.sprite = item.itemData.icon;
            }
            else
            {
                _icon.sprite = _currencySprite;
            }
        }

        public void Select()
        {
            
        }
        
        public void Deselect()
        {
            
        }
    }
}