using System.Collections.Generic;
using System.Linq;
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

        public int GetPriceForItem(ItemType type, SimpleFreshnessLevel freshness)
        {
            var item = _items.First(item => item.itemType == type);
            switch (freshness)
            {
                case SimpleFreshnessLevel.PERFECT:
                    return item.priceWhenPerfect;
                case SimpleFreshnessLevel.GOOD:
                    return item.priceWhenGood;
                default:
                    return item.priceWhenBad;
            }
        }

        public bool IsIllegal(ItemType type)
        {
            return _items.First(item => item.itemType == type).illegal;
        }
    }
}