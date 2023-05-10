using System;
using Code.GameScene.UI;
using DG.Tweening;
using General;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class LevelButton : MonoBehaviour
    {
        private const float ScaleTimeInSeconds = 0.3f;
        private const float ClickScaleTimeInSeconds = 0.15f;
        private const float HoverScale = 1.15f;
        private const float ClickScale = 1.3f;

        private bool _hovering;
        
        public int level;
        public bool canBePlayed;

        public Sprite enabledSprite;
        public Sprite unenabledSprite;
        public TextMeshPro levelText;
        
        public SpriteRenderer spriteRenderer;
        public Collider2D _collider;

        public void Start()
        {
            levelText.text = level.ToString();
        }
        
        public void Trigger()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }
        
        public void SetAvailable()
        {
            _collider.enabled = true;
            canBePlayed = true;
            spriteRenderer.sprite = enabledSprite;
            levelText.alpha = 1f;
        }

        public void SetInavailable()
        {
            _collider.enabled = false;
            canBePlayed = false;
            spriteRenderer.sprite = unenabledSprite;
            levelText.alpha = 0.2f;
        }

        private void OnMouseEnter()
        {
            _hovering = true;
            if (IsEnabled())
            {
                LevelChoosingSceneManager.instance.SetActiveButton(level);
            }
        }

        private void OnMouseExit()
        {
            _hovering = false;
        }

        private void OnMouseUp()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }
        
        public void OnClick()
        {
            SceneManager.Get().StartLevel(level);
            AudioManager.instance.PlayBlub();
            DOTween.Sequence()
                .Append(ScaleUpOnClick())
                .Append(ScaleDownAfterClick())
                .Play();
        }

        public bool IsEnabled()
        {
            return canBePlayed && !OptionScreen.instance.IsVisible() &&!SceneManager.Get().IsInTransition();
        }

        public void OnSetDeactive()
        {
            ScaleDown();
        }

        public void OnSetActive()
        {
            ScaleUp();
        }
        
        protected Tween ScaleUpOnClick()
        {
            return transform.DOScale(ClickScale, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        protected Tween ScaleDownAfterClick()
        {
            return transform.DOScale(_hovering ? HoverScale : 1f, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private Tween ScaleUp()
        {
            return transform.DOScale(HoverScale, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private Tween ScaleDown()
        {
            return transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }
    }
}