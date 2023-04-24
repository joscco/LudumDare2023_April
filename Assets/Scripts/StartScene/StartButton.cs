using System;
using Code;
using Code.GameScene.UI;

namespace StartScene
{
    public class StartButton : ScalingButton
    {
        public String levelChooserSceneName = "LevelChoosingScene";
        
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionTo(levelChooserSceneName);
        }
    }
}
