using System.Collections.Generic;
using GameScene.BuildingMap;
using GameScene.Inventory;
using LevelDesign;
using UnityEngine;

namespace GameScene.Buildings
{
    public class SupplierBuildingManager : BuildingManager<SupplierBuilding>
    {
        [SerializeField] private SupplierBuilding pizzaShopPrefab;
        [SerializeField] private SupplierBuilding sushiShopPrefab;
        [SerializeField] private SupplierBuilding burgerShopPrefab;
        [SerializeField] private SupplierBuilding weaponShopPrefab;
        [SerializeField] private SupplierBuilding drugShopPrefab;
        [SerializeField] private SupplierBuilding organShopPrefab;

        public void InitSuppliers(List<SupplierData> suppliers)
        {
            foreach (var supplier in suppliers)
            {
                SupplierBuilding building;
                switch (supplier.Type)
                {
                    case ItemType.PIZZA:
                        building = Instantiate(pizzaShopPrefab, transform);
                        break;
                    case ItemType.BURGER:
                        building = Instantiate(burgerShopPrefab, transform);
                        break;
                    case ItemType.SUSHI:
                        building = Instantiate(sushiShopPrefab, transform);
                        break;
                    case ItemType.DRUGS:
                        building = Instantiate(drugShopPrefab, transform);
                        break;
                    case ItemType.WEAPON:
                        building = Instantiate(weaponShopPrefab, transform);
                        break;
                    default:
                        building = Instantiate(organShopPrefab, transform);
                        break;
                }
                
                building.itemsInSupply = supplier.SuppliedItems;
                building.suppliedItemType = supplier.Type;
                building.InitHint(supplier.SignPosition, building.itemsInSupply);
                AddBuildingAt(building, supplier.Position);
                
            }
        }
    }
}