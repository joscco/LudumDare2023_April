using GameScene.BuildingMap;
using GameScene.Buildings;
using UnityEngine;

namespace GameScene.Inventory
{
    [CreateAssetMenu(fileName = "New InventoryItem", menuName = "InventoryItemData")]
    public class InventoryItem : ScriptableObject
    {
        public ItemType itemType;
        public BuildingType supplierType;
        public Sprite inventorySprite;
    }
}