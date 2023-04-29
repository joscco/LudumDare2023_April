using GameScene.BuildingMap;
using GameScene.Inventory;

namespace GameScene.Buildings
{
    public class ConsumerBuilding : Building
    {
        // For Consumer
        public bool demandsItem;
        public ItemType demandedItemType;

        public void SetSatisfied()
        {
            demandsItem = false;
        }
    }
}