using System;
using DG.Tweening;
using GameScene.UI;
using UnityEngine;

namespace Code.GameScene.UI
{
    public abstract class CustomSlider : MonoBehaviour
    {
        public SpriteRenderer background;
        public SpriteRenderer knob;
        public Camera camera;

        private Tween knobTween;
        private float value;
        private bool SliderKnob;
        
        private bool dragging;

        private void OnMouseDown()
        {
            OptionScreen.instance.SetActiveSlider(this);
            UpdateFromMouse();
        }

        private void OnMouseDrag()
        {
            UpdateFromMouse();
        }

        private void UpdateFromMouse()
        {
            var globalPos = camera.ScreenToWorldPoint(Input.mousePosition);
            var localPos = globalPos - background.transform.position;
            AdaptValue(localPos.x);
        }

        private void AdaptValue(float xPositionLocal)
        {
            var sanitizedPosition = Math.Clamp(xPositionLocal, 0, background.sprite.texture.width);
            var valueForPosition = sanitizedPosition / background.sprite.texture.width;
            ChangeValue(valueForPosition);
        }

        public void ChangeValue(float newValue)
        {
            value = Math.Clamp(newValue, 0, 1);
            knob.transform.localPosition =
                new Vector2( value * background.sprite.texture.width, 0);
            OnChangeValue(value);
        }

        public float GetValue()
        {
            return value;
        }

        public abstract void OnChangeValue(float value);

        public void OnSetActive()
        {
            knobTween?.Kill();
            knobTween = knob.transform.DOScale(1.1f, 0.2f).SetEase(Ease.InOutQuad);
        }
        
        public void OnSetDeactive()
        {
            knobTween?.Kill();
            knobTween = knob.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad);
        }
    }
}