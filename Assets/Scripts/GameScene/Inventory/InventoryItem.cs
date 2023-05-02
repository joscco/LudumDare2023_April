using GameScene.BuildingMap;
using GameScene.Buildings;
using UnityEngine;

namespace GameScene.Inventory
{
    [CreateAssetMenu(fileName = "New InventoryItem", menuName = "InventoryItemData")]
    public class InventoryItem : ScriptableObject
    {
        public ItemType itemType;
        public bool illegal;
        
        public BuildingType supplierType;
        public Sprite inventorySprite;

        public int priceWhenPerfect;
        public int priceWhenGood;
        public int priceWhenBad;
    }
}