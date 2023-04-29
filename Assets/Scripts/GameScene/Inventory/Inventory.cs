using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.BuildingMap;
using GameScene.Buildings;
using UnityEngine;

namespace GameScene.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int numberOfSlots;
        public int slotHeight = 100;
        public int slotVerticalGap = 20;
        [SerializeField] private InventorySlot slotPrefab;
        [SerializeField] private InventorySpriteWiki spriteWiki;
        
        private List<InventorySlot> _slots;

        private void Start()
        {
            _slots = new List<InventorySlot>();
            for (int i = 0; i < numberOfSlots; i++)
            {
                var slot = Instantiate(slotPrefab, transform);
                slot.transform.localPosition = new Vector2(0, - i * (slotHeight + slotVerticalGap));
                _slots.Add(slot);
            }
        }

        public void AddItem(ItemType type)
        {
            if (HasFreeSlot())
            {
                Debug.Log("Adding Item!");
                var slot = GetFreeSlot();
                var sprite = spriteWiki.FindSpriteForType(type);
                slot.SetItem(type, sprite);
            }
            else
            {
                Debug.LogError("No free slot available!");
            }
        }

        public void RemoveItem(ItemType type)
        {
            if (HasItem(type))
            {
                var slot = _slots.Where(slot => slot.HasItem()).First(slot => slot.GetType() == type);
                slot.RemoveItem();
            }
            else
            {
                Debug.LogError("Item not available!");
            }
        }

        public bool HasItem(ItemType type)
        {
            return _slots.Where(slot => slot.HasItem()).Any(slot => slot.GetType() == type);
        }

        public bool HasFreeSlot()
        {
            return _slots.Any(slot => !slot.HasItem());
        }

        private InventorySlot GetFreeSlot()
        {
            return _slots.First(slot => !slot.HasItem());
        }

        public ItemType GetItemForBuildingType(BuildingType buildingType)
        {
            return spriteWiki.FindItemForSupplier(buildingType);
        }
    }
}