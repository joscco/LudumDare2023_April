using Code.GameScene.UI;

namespace GameScene.UI
{
    public class LevelSceneBackButton : ScalingButton
    {
        public string sceneName = "StartScene";
        
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