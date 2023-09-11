using System.Collections;
using System.Collections.Generic;
using Features.Inventory.Item;
using UnityEngine;

namespace Features.Drop
{
    public class DropInstance : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        
        private List<InventoryItem> _items;
        private int _currency;

        public void Configure(List<InventoryItem> items, int currency)
        {
            _items = items;
            _currency = currency;
            _indicator.SetActive(false);
        }
        
        public void SetDetected(bool isDetected)
        {
            _indicator.SetActive(isDetected);
        }

        public (List<InventoryItem>, int) GetDrops()
        {
            return (_items, _currency);
        }
        
        public void OnGrabItem(InventoryItem item)
        {
            _items.Remove(item);
            StartCoroutine(CheckForDestruction());
        }
        
        public void OnGrabCurrency()
        {
            _currency = 0;
            StartCoroutine(CheckForDestruction());
        }
        
        private IEnumerator CheckForDestruction()
        {
            yield return new WaitForEndOfFrame();
            
            if (_items.Count == 0 && _currency == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}