using DG.Tweening;
using UnityEngine;

namespace GameScene.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer itemRenderer;

        private bool _hasItem;
        private ItemType _type;

        private void Start()
        {
            itemRenderer.transform.localScale = Vector3.zero;
        }

        public bool HasItem()
        {
            return _hasItem;
        }

        public new ItemType GetType()
        {
            return _type;
        }

        public void SetItem(ItemType type, Sprite sprite)
        {
            _hasItem = true;
            _type = type;
            SetSprite(sprite);
            BlendInItem();
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