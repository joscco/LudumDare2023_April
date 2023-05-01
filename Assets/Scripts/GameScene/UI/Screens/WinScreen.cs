using DG.Tweening;
using UnityEngine;

namespace GameScene.UI
{
    public class WinScreen : MonoBehaviour
    {
        public TitleAnimation titleAnimation;
        private const int VERTICAL_OFFSET_WHEN_HIDDEN = 900;
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
        }

        public void BlendOut()
        {
            _visible =false;
            transform.DOMoveY(-VERTICAL_OFFSET_WHEN_HIDDEN, 0.5f).SetEase(Ease.InBack);
        }

        public void Hide()
        {
            _visible =false;
            transform.position = new Vector2(0, -VERTICAL_OFFSET_WHEN_HIDDEN);
        }

        public bool IsVisible()
        {
            return _visible;
        }
    }
}