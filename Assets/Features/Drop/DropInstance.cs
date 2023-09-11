using System.Collections;
using Features.Inventory.Item;
using UnityEngine;

namespace Features.Drop
{
    public class DropInstance : MonoBehaviour
    {
        private InventoryItem _item;
        private int _currency;
        
        public void Configure(InventoryItem item, int currency)
        {
            _item = item;
            _currency = currency;
        }

        public (InventoryItem, int) GetDrops()
        {
            return (_item, _currency);
        }
        
        public InventoryItem GrabItem()
        {
            var item = _item;
            _item = null;
            StartCoroutine(CheckForDestruction());
            return item;
        }
        
        public int GrabCurrency()
        {
            var currency = _currency;
            _currency = 0;
            StartCoroutine(CheckForDestruction());
            return currency;
        }
        
        private IEnumerator CheckForDestruction()
        {
            yield return new WaitForEndOfFrame();
            
            if (_item == null && _currency == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}