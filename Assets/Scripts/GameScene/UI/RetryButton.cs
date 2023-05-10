using Code.GameScene.UI;

namespace GameScene
{
    public class RetryButton : ScalingButton
    {
        public override void OnClick()
        {
            SceneTransitionManager.Get().ReloadCurrentLevel();
        }

        public override bool IsEnabled()
        {
            return !SceneTransitionManager.Get().IsInTransition();
        }
    }
}