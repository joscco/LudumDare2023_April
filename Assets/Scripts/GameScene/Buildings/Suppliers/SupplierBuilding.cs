using GameScene.Buildings;
using GameScene.Inventory;
using LevelDesign;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.BuildingMap
{
    public class SupplierBuilding : Building
    {
        public int itemsInSupply;
        public ItemType suppliedItemType;

        public bool CanSupply()
        {
            return itemsInSupply > 0;
        }

        public void TakeItem()
        {
            itemsInSupply--;

            if (itemsInSupply < 0)
            {
                Debug.LogError("More Items were taken then available!");
            }

            itemHint.UpdateNumber(itemsInSupply);

            if (itemsInSupply <= 0)
            {
                itemHint.BlendOut();
            }
        }

        public override bool NeedsHint()
        {
            return CanSupply();
        }

        public override ItemType GetItemType()
        {
            return suppliedItemType;
        }

        public void BlendInHint()
        {
            itemHint.BlendIn();
        }
        
        public void HideItem()
        {
            itemHint.Hide();
        }
    }
}