using DG.Tweening;
using UnityEngine;

namespace GameScene.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer itemRenderer;
        [SerializeField] private FreshnessDisplay _freshnessDisplay;

        private bool _hasItem;
        private ItemType _type;

        private void Start()
        {
            itemRenderer.transform.localScale = Vector3.zero;
            _freshnessDisplay.Hide();
        }

        public bool HasItem()
        {
            return _hasItem;
        }

        public void NextStatus()
        {
            if (_hasItem)
            {
                _freshnessDisplay.NextStatus();
            }
        }

        public new ItemType GetType()
        {
            return _type;
        }

        public SimpleFreshnessLevel GetFreshness()
        {
            return _freshnessDisplay.GetFreshness();
        }

        public void SetItem(ItemType type, Sprite sprite)
        {
            _hasItem = true;
            _type = type;
            SetSprite(sprite);
            BlendInItem();

            if (type == ItemType.DRUGS || type == ItemType.WEAPON)
            {
                _freshnessDisplay.SetInfiniteFresh();
            }
            else
            {
                _freshnessDisplay.SetRegularFresh();
            }
            _freshnessDisplay.BlendIn();
        }

        private void SetSprite(Sprite sprite)
        {
            itemRenderer.sprite = sprite;
        }

        public void RemoveItem()
        {
            _hasItem = false;
            _type = ItemType.PIZZA;
            BlendOutItem();
            _freshnessDisplay.BlendOut();
        }

        private void BlendInItem()
        {
            itemRenderer.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }
        
        private void BlendOutItem()
        {
            itemRenderer.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
    }
}