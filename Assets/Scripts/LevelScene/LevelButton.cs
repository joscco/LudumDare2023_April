using System;
using Code.GameScene.UI;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class LevelButton : ScalingButton
    {
        public int level;
        public bool canBePlayed;

        public Sprite enabledSprite;
        public Sprite unenabledSprite;
        public TextMeshPro levelText;
        
        public SpriteRenderer spriteRenderer;
        public Collider2D _collider;

        public override void Start()
        {
            base.Start();
            levelText.text = level.ToString();
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


        public override void OnClick()
        {
            SceneTransitionManager.Get().StartLevel(level);
        }

        public override bool IsEnabled()
        {
            return canBePlayed && !OptionScreen.instance.IsVisible() &&!SceneTransitionManager.Get().IsInTransition();
        }
    }
}