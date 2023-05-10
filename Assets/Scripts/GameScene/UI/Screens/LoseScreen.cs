using System;
using Code.GameScene.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class LoseScreen : MonoBehaviour
    {
        public Sprite mafiaReasonSprite;
        public Sprite policeReasonSprite;
        public Sprite noActionsLeftSprite;
        public TextMeshPro reasonText;
        public SpriteRenderer reasonSpriteRenderer;
        public TitleAnimation titleAnimation;

        public RetryButton retryButton;
        public GameScreenBackButton backButton;

        private const int VerticalOffsetWhenHidden = 1200;
        private bool _visible;
        
        private ScalingButton _activeButton;
        private ScalingButton[] _availableButtons;

        private void Start()
        {
            Hide();
            _availableButtons = new ScalingButton[] { retryButton, backButton };
            SetActiveButton(_availableButtons[0]);
        }
        
        public void SetActiveButton(ScalingButton button)
        {
            if (_activeButton)
            {
                _activeButton.OnSetDeactive();
            }
            _activeButton = button;
            _activeButton.OnSetActive();
        }

        public void BlendIn(LoseReason reason)
        {
            UpdateReasonSprite(reason);
            UpdateReasonText(reason);
            _visible = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBack);
            titleAnimation.Hide();
            titleAnimation.FadeIn();
        }

        private void UpdateReasonSprite(LoseReason reason)
        {
            switch (reason)
            {
                case LoseReason.NoActionsLeft:
                    reasonText.text = "Oh no! You ran out of actions.";
                    break;
                case LoseReason.Mafia:
                    reasonText.text = "Seems the mafia wasn't satisfied with you.";
                    break;
                case LoseReason.Police:
                    reasonText.text = "Hands Up! You got caught by the police.";
                    break;
            }
        }

        private void UpdateReasonText(LoseReason reason)
        {
            switch (reason)
            {
                case LoseReason.NoActionsLeft:
                    reasonSpriteRenderer.sprite = noActionsLeftSprite;
                    break;
                case LoseReason.Mafia:
                    reasonSpriteRenderer.sprite = mafiaReasonSprite;
                    break;
                case LoseReason.Police:
                    reasonSpriteRenderer.sprite = policeReasonSprite;
                    break;
            }
        }

        public void BlendOut()
        {
            _visible = false;
            transform.DOMoveY(-VerticalOffsetWhenHidden, 0.5f).SetEase(Ease.InBack);
        }

        public void Hide()
        {
            _visible =false;
            transform.position = new Vector2(0, -VerticalOffsetWhenHidden);
        }

        public bool IsVisible()
        {
            return _visible;
        }

        public void HandleMoveInput(Vector2Int move)
        {
            if (move.x != 0 && _activeButton)
            {
                SetActiveButton(FindNextAvailableButton());
            }
        }

        private ScalingButton FindNextAvailableButton()
        {
            return _availableButtons[(Array.IndexOf(_availableButtons, _activeButton) + 1) % _availableButtons.Length];
        }

        public void OnPressEnter()
        {
            if (_activeButton)
            {
                _activeButton.Trigger();
            }
        }
    }

    public enum LoseReason
    {
        NoActionsLeft, Mafia, Police
    }
}