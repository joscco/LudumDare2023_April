using System;
using Code.GameScene.UI;
using DG.Tweening;
using UnityEngine;

namespace GameScene.UI.Screens
{
    public class WinScreen : MonoBehaviour
    {
        public TitleAnimation titleAnimation;
        public NextLevelButton nextLevelButton;
        public GameScreenBackButton backButton;
        
        private const int VerticalOffsetWhenHidden = 1200;
        private bool _visible;

        private ScalingButton _activeButton;
        private ScalingButton[] _availableButtons;

        private void Start()
        {
            Hide();
        }

        public void BlendIn()
        {
            _visible = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBack);
            
            titleAnimation.Hide();
            titleAnimation.FadeIn();
             
            if (SceneManager.Get().GetCurrentLevel() >= Game.AVAILABLE_LEVELS)
            {
                nextLevelButton.enabled = false;
                nextLevelButton.transform.localScale = Vector3.zero;
                backButton.transform.position = new Vector2(0, backButton.transform.position.y);
                _availableButtons = new ScalingButton[] { backButton };
            }
            else
            {
                _availableButtons = new ScalingButton[] { nextLevelButton, backButton };
            }
            SetActiveButton(_availableButtons[0]);
        }

        public void BlendOut()
        {
            _visible = false;
            transform.DOMoveY(-VerticalOffsetWhenHidden, 0.5f).SetEase(Ease.InBack);
        }

        public void Hide()
        {
            _visible = false;
            transform.position = new Vector2(0, -VerticalOffsetWhenHidden);
        }

        public bool IsVisible()
        {
            return _visible;
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
}