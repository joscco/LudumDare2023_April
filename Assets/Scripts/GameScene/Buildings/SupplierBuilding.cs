using System.Collections.Generic;
using GameScene.Buildings;
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
        }

        public static readonly List<BuildingType> SUPPLIERS = new()
        {
            BuildingType.PIZZA, BuildingType.SUSHI, BuildingType.BURGER, BuildingType.TACO,
            BuildingType.HOSPITAL, BuildingType.DRUG_DEALER
        };
    }
}