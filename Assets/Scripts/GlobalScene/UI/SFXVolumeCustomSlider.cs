using System;
using General;

namespace Code.GameScene.UI
{
    public class SFXVolumeCustomSlider : CustomSlider
    {
        public AudioManager audioManager;

        private void Start()
        {
            ChangeValue(audioManager.GetSFXVolume());
        }

        public override void OnChangeValue(float value)
        {
            audioManager.SetSFXVolume(value);
        }
    }
}