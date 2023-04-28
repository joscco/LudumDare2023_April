using Code.GameScene.UI;

namespace GameScene.UI
{
    public class GameScreenBackButton : ScalingButton
    {
        public string sceneName = "LevelChooserScene";
        
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionTo(sceneName);
        }

        public override bool IsEnabled()
        {
            return !SceneTransitionManager.Get().IsInTransition();
        }
    }
}