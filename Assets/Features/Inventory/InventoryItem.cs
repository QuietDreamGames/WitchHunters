using System;
using UnityEngine;

namespace Features.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public string id;
        public string displayName;
        public Sprite icon;
    }
}