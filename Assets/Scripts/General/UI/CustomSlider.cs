using System;
using UnityEngine;

namespace Code.GameScene.UI
{
    public abstract class CustomSlider : MonoBehaviour
    {
        public SpriteRenderer background;
        public SpriteRenderer knob;
        public Camera camera;
        
        private float value;
        private bool SliderKnob;
        
        private bool dragging;

        private void OnMouseDown()
        {
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

        protected void ChangeValue(float value)
        {
            knob.transform.localPosition =
                new Vector2( value * background.sprite.texture.width, 0);
            OnChangeValue(value);
        }

        public abstract void OnChangeValue(float value);
    }
    
    
}