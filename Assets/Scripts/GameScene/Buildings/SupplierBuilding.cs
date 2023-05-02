using System.Collections.Generic;
using GameScene.Buildings;
using GameScene.Inventory;
using UnityEngine;

namespace GameScene.BuildingMap
{
    public class SupplierBuilding : Building
    {
        // For Suppliers

        public int itemsInSupply;

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

            if (itemsInSupply <= 0)
            {
                itemHint.BlendOut();
            }
        }

        public static readonly List<BuildingType> SUPPLIERS = new()
        {
            BuildingType.PIZZA_SHOP, BuildingType.SUSHI_SHOP, BuildingType.BURGER_SHOP,
            BuildingType.HOSPITAL, BuildingType.WEAPON_SHOP, BuildingType.DRUG_SHOP
        };

        public override bool NeedsHint()
        {
            return itemsInSupply > 0;
        }

        public override ItemType GetItemType()
        {
            switch (type)
            {
                case BuildingType.PIZZA_SHOP:
                    return ItemType.PIZZA;
                case BuildingType.BURGER_SHOP:
                    return ItemType.BURGER;
                case BuildingType.SUSHI_SHOP:
                    return ItemType.SUSHI;
                case BuildingType.WEAPON_SHOP:
                    return ItemType.WEAPON;
                case BuildingType.DRUG_SHOP:
                    return ItemType.DRUGS;
                default:
                    return ItemType.ORGAN;
            }
        }

        public void BlendInHint()
        {
            itemHint.BlendIn();
        }
    }
}