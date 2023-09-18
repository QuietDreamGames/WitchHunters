using Features.Inventory;
using Features.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Gameplay.Loot
{
    public class LootItemController : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedBackground;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _amountText;
        
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
                _nameText.text = item.itemData.name;
                
                if (item.itemData.sortType == ItemSortType.Scrap)
                    _amountText.text = item.amount.ToString();
                else
                    _amountText.text = "";
            }
            else
            {
                _icon.sprite = _currencySprite;
                _nameText.text = "Gold";
                _amountText.text = currency.ToString();
            }
        }

        public void Select()
        {
            _selectedBackground.enabled = true;
        }
        
        public void Deselect()
        {
            _selectedBackground.enabled = false;
        }
    }
}