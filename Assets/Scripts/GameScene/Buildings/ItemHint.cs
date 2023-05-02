using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.Buildings
{
    public class ItemHint : MonoBehaviour
    {
        private Tween _jumpTween;
        private Tween _scaleTween;
        public SpriteRenderer itemRenderer;
        private int _numberOfItems = 0;
        private float initialPosY;

        private void Start()
        {
            initialPosY = transform.position.y;
        }

        public void BlendIn()
        {
            _jumpTween.Kill();
            _scaleTween.Kill();
            _scaleTween = transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            StartJumping();
        }

        public void BlendOut()
        {
            _jumpTween.Kill();
            _scaleTween.Kill();
            _scaleTween = transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }

        public void Show()
        {
            _jumpTween.Kill();
            _scaleTween.Kill();
            transform.localScale = Vector3.one;
            StartJumping();
        }

        public void Hide()
        {
            _jumpTween.Kill();
            _scaleTween.Kill();
            transform.localScale = Vector3.zero;
        }

        public void StartJumping()
        {
            _jumpTween = transform.DOMoveY(initialPosY + 20, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        }

        public void SetSprite(Sprite sprite)
        {
            itemRenderer.sprite = sprite;
        }
    }
}