using GameScene.Inventory;
using UnityEngine.Serialization;

namespace GameScene.Buildings
{
    public class ConsumerBuilding : Building
    {
        // For Consumer
        public int demandedItems;
        public ItemType demandedItemType;

        public void SetSatisfied()
        {
            demandedItems = 0;
            itemHint.UpdateNumber(demandedItems);
            itemHint.BlendOut();
        }

        public override bool NeedsHint()
        {
            return demandedItems > 0;
        }

        public override ItemType GetItemType()
        {
            return demandedItemType;
        }

        public override int GetAdditionalVerticalOffset()
        {
            return 10;
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