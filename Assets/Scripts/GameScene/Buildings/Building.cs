using System;
using GameScene.Inventory;
using UnityEngine;

namespace GameScene.Buildings
{
    public abstract class Building: MonoBehaviour
    {
        public BuildingType type;
        public ItemHint itemHint;

        // Will be set at start by grid
        private Vector2Int _index;

        private void Start()
        {
            itemHint.Hide();
        }

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

        public void BlendOutHint()
        {
            itemHint.BlendOut();
        }
    }

    public enum BuildingType
    {
        HOUSE, POLICE, PIZZA_SHOP, SUSHI_SHOP, BURGER_SHOP, HOSPITAL, WEAPON_SHOP, DRUG_SHOP
    }
}