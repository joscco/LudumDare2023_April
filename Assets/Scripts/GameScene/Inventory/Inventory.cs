using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int numberOfSlots;
        public int slotHeight = 100;
        public int slotVerticalGap = 20;
        [SerializeField] private InventorySlot slotPrefab;
        
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

        public void AddItem(ItemType type, Sprite sprite)
        {
            if (HasFreeSlot())
            {
                var slot = GetFreeSlot();
                slot.SetItem(type, sprite);
            }
            else
            {
                Debug.LogError("No free slot available!");
            }
        }

        public SimpleFreshnessLevel RemoveItem(ItemType type)
        {
            if (HasItem(type))
            {
                var slot = _slots.Where(slot => slot.HasItem()).First(slot => slot.GetType() == type);
                var freshness = slot.GetFreshness();
                slot.RemoveItem();
                return freshness;
            }
            
            Debug.LogError("Item not available!");
            return SimpleFreshnessLevel.BAD;
        }
        
        public void RemoveFirstFood()
        {
            var slot = _slots.Where(slot => slot.HasItem()).First(slot => Food.Contains(slot.GetType()));
            if (null != slot)
            {
                slot.RemoveItem();
            }
        }

        public void NextStatus()
        {
            _slots.Where(slot => slot.HasItem()).ToList().ForEach(slot => slot.NextStatus());
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

        public bool HasBadHeart()
        {
            return _slots.Any(slot =>
                slot.HasItem() && slot.GetType() == ItemType.ORGAN && slot.GetFreshness() == SimpleFreshnessLevel.BAD);
        }

        public bool HoldsIllegalItem()
        {
            return _slots.Any(slot => slot.HasItem() && IllegalItems.Contains(slot.GetType()));
        }
        
        public bool HoldsFood()
        {
            return _slots.Any(slot => slot.HasItem() && Food.Contains(slot.GetType()));
        }

        private static readonly ItemType[] IllegalItems = { ItemType.DRUGS, ItemType.WEAPON, ItemType.ORGAN };

        private static readonly ItemType[] Food = { ItemType.PIZZA, ItemType.SUSHI, ItemType.BURGER };
    }
}