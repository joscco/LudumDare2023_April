using DG.Tweening;
using LevelDesign;
using TMPro;
using UnityEngine;

namespace GameScene.Buildings
{
    public class ItemHint : MonoBehaviour
    {
        private Tween _jumpTween;
        private Tween _scaleTween;
        public SpriteRenderer backgroundRenderer;
        public SpriteRenderer itemRenderer;
        public SpriteRenderer numberBackRenderer;
        public TextMeshPro numberRenderer;
        private int _numberOfItems = 0;

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
            var initialPosY = transform.position.y;
            _jumpTween = transform.DOMoveY(initialPosY + 20, 1f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        public void StopJumping()
        {
            _jumpTween?.Kill();
        }

        public void SetSprite(Sprite sprite)
        {
            itemRenderer.sprite = sprite;
        }

        public void UpdateNumber(int number)
        {
            if (number == 0)
            {
                numberBackRenderer.color = new Color(0.99f, 0.81f, 0.58f, 1f);
                numberRenderer.color = Color.black;
                numberRenderer.text = number.ToString();
            }
            else if (number > 0)
            {
                numberRenderer.text = "+" + number.ToString();
                numberRenderer.color = Color.white;
                numberBackRenderer.color = new Color(0.18f, 0.56f, 0.34f, 1f);
            }
            else if (number < 0)
            {
                numberRenderer.text = number.ToString();
                numberRenderer.color = Color.white;
                numberBackRenderer.color = new Color(0.99f, 0.47f, 0.47f, 1f);
            }

            DOTween.Sequence()
                .Append(numberRenderer.transform
                    .DOScale(1.2f, 0.2f)
                    .SetEase(Ease.OutBack))
                .Append(numberRenderer.transform
                    .DOScale(1f, 0.2f)
                    .SetEase(Ease.OutBack));
        }

        private void TurnBackground(int degrees)
        {
            backgroundRenderer.transform.rotation = Quaternion.Euler(0, 0, degrees);
        }

        public void Turn(SignPosition supplierSignPosition)
        {
            StopJumping();
            switch (supplierSignPosition)
            {
                case SignPosition.Top:
                    TurnBackground(0);
                    transform.localPosition = Vector2.up * 125;
                    break;
                case SignPosition.Bottom:
                    TurnBackground(180);
                    transform.localPosition = Vector2.down * 100;
                    break;
                case SignPosition.Left:
                    TurnBackground(90);
                    transform.localPosition = Vector2.left * 100;
                    break;
                case SignPosition.Right:
                    TurnBackground(-90);
                    transform.localPosition = Vector2.right * 100;
                    break;
            }
            StartJumping();
        }
    }
}