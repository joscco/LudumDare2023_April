using General;

namespace Code.GameScene.UI
{
    public class MusicVolumeCustomSlider : CustomSlider
    {
        public AudioManager audioManager;
        
        private void Start()
        {
            ChangeValue(audioManager.GetMusicVolume());
        }
        public override void OnChangeValue(float value)
        {
            audioManager.SetMusicVolume(value);
        }
    }
}