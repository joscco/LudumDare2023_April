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
            itemHint.BlendOut();
        }

        public override bool NeedsHint()
        {
            return demandsItem;
        }

        public override ItemType GetItemType()
        {
            return demandedItemType;
        }

        public void BlendInHint()
        {
            itemHint.BlendIn();
        }
    }
}