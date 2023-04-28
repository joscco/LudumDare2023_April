using System;
using Code;
using Code.GameScene.UI;
using General;

namespace StartScene
{
    public class StartButton : ScalingButton
    {
        public String levelChooserSceneName = "LevelChoosingScene";
        private bool _clickable;
        
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionTo(levelChooserSceneName);
            AudioManager.instance.PlayMusic();
            _clickable = false;
        }

        public void Start()
        {
            _clickable = true;
        }

        public override bool IsEnabled()
        {
            return _clickable;
        }
    }
}
