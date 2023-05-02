using DG.Tweening;
using UnityEngine;

namespace GameScene.UI
{
    public class WinScreen : MonoBehaviour
    {
        public TitleAnimation titleAnimation;
        public NextLevelButton nextLevelButton;
        private const int VERTICAL_OFFSET_WHEN_HIDDEN = 1200;
        private bool _visible = false;

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
            if (SceneTransitionManager.Get().GetCurrentLevel() >= Game.AVAILABLE_LEVELS)
            {
                nextLevelButton.enabled = false;
                nextLevelButton.transform.localScale = Vector3.zero;
                
            }
        }

        public void BlendOut()
        {
            _visible = false;
            transform.DOMoveY(-VERTICAL_OFFSET_WHEN_HIDDEN, 0.5f).SetEase(Ease.InBack);
        }

        public void Hide()
        {
            _visible = false;
            transform.position = new Vector2(0, -VERTICAL_OFFSET_WHEN_HIDDEN);
        }

        public bool IsVisible()
        {
            return _visible;
        }
    }
}