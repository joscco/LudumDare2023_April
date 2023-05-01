using System;
using DG.Tweening;
using General;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameScene.UI
{
    public class OptionScreen : MonoBehaviour
    {
        private const int VERTICAL_OFFSET_WHEN_HIDDEN = 1000;
        private bool _visible = false;
        public static OptionScreen instance;

        private void Start()
        {
            instance = this;
            Hide();
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

        public void SetMusicVolume(float value)
        {
            AudioManager.instance.SetMusicVolume(value);
        }
        
        public void SetSFXVolume(float value)
        {
            AudioManager.instance.SetSFXVolume(value);
        }
    }
}