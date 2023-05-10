using System;
using GameScene.Inventory;
using LevelDesign;
using UnityEngine;

namespace GameScene.Buildings
{
    public abstract class Building: MonoBehaviour
    {
        public ItemHint itemHint;

        // Will be set at start by grid
        private Vector2Int _index;

        public void SetIndex(Vector2Int index)
        {
            _index = index;
        }

        public Vector2Int GetIndex()
        {
            return _index;
        }

        public abstract bool NeedsHint();

        public abstract ItemType GetItemType();

        public abstract int GetAdditionalVerticalOffset();

        public void InitHint(SignPosition supplierSignPosition, int number)
        {
            itemHint.Turn(supplierSignPosition, GetAdditionalVerticalOffset());
            itemHint.UpdateNumber(number);
        }

    }
}