using Code.GameScene.UI;

namespace GameScene
{
    public class RetryButton : ScalingButton
    {
        private bool _fadeIn;
        public override void OnClick()
        {
            SceneTransitionManager.Get().ReloadCurrentScene();
        }

        public override bool IsEnabled()
        {
            return !SceneTransitionManager.Get().IsInTransition();
        }
    }
}