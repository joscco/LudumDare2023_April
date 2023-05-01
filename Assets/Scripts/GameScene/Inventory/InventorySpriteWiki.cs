using System.Collections.Generic;
using System.Linq;
using GameScene.Buildings;
using UnityEngine;

namespace GameScene.Inventory
{
    public class InventorySpriteWiki : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> _items;

        public Sprite FindSpriteForType(ItemType type)
        {
            return _items.First(item => item.itemType == type).inventorySprite;
        }

        public ItemType FindItemForSupplier(BuildingType type)
        {
            return _items.First(item => item.supplierType == type).itemType;
        }
    }
}