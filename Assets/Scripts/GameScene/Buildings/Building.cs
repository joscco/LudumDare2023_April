using System;
using System.Collections.Generic;
using GameScene.Inventory;
using UnityEngine;

namespace GameScene.BuildingMap
{
    public class Building: MonoBehaviour
    {
        public BuildingType type;

        // For Suppliers

        public int itemsInSupply;

        // For Consumer
        public bool demandsItem;
        public ItemType demandedItemType;
        
        // Will be set at start by grid
        private Vector2Int _index;

        public bool IsSupplier()
        {
            return SUPPLIERS.Contains(type);
        }

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
        }
        
        public static readonly List<BuildingType> SUPPLIERS = new()
        {
            BuildingType.PIZZA, BuildingType.SUSHI, BuildingType.BURGER, BuildingType.TACO,
            BuildingType.HOSPITAL, BuildingType.DRUG_DEALER
        };

        public void SetIndex(Vector2Int index)
        {
            _index = index;
        }

        public Vector2Int GetIndex()
        {
            return _index;
        }
    }

    public enum BuildingType
    {
        HOUSE, PIZZA, SUSHI, BURGER, TACO, HOSPITAL, POLICE, DRUG_DEALER
    }
}