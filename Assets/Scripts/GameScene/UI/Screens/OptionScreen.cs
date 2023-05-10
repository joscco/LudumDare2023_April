using Code.GameScene.UI;
using DG.Tweening;
using UnityEngine;

namespace GameScene.UI
{
    public class OptionScreen : MonoBehaviour
    {
        [SerializeField] private MusicVolumeCustomSlider musicSlider;
        [SerializeField] private SFXVolumeCustomSlider sfxSlider;
        [SerializeField] private OptionButton _optionButton;

        private const int VERTICAL_OFFSET_WHEN_HIDDEN = 1000;
        private bool _visible;
        public static OptionScreen instance;
        private CustomSlider activeSlider;

        private void Start()
        {
            instance = this;
            Hide();
            SetActiveSlider(musicSlider);
        }

        public void SetActiveSlider(CustomSlider slider)
        {
            if (activeSlider)
            {
                activeSlider.OnSetDeactive();
            }
            activeSlider = slider;
            activeSlider.OnSetActive();
        }

        public void Toggle()
        {
            if (IsVisible())
            {
                BlendOut();
            }
            else
            {
                BlendIn();
            }
        }

        public void BlendIn()
        {
            _visible = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBack);
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

        public void HandleMoveInput(Vector2Int move)
        {
            if (move == Vector2Int.zero)
            {
                return;
            }
            
            if (activeSlider == musicSlider && move.y == -1)
            {
                SetActiveSlider(sfxSlider);
            } else if (activeSlider == sfxSlider && move.y == 1)
            {
                SetActiveSlider(musicSlider);
            }
            else if (activeSlider)
            {
                activeSlider.ChangeValue(activeSlider.GetValue() + 0.15f * move.x);
            }
        }

        public void OnPressEnter()
        {
            _optionButton.Trigger();
        }
    }
}