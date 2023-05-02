using Code.GameScene.UI;

namespace GameScene
{
    public class NextLevelButton : ScalingButton
    {
        private bool _fadeIn;
        public override void OnClick()
        {
            SceneTransitionManager.Get().LoadNextLevel();
        }

        public override bool IsEnabled()
        {
            return !SceneTransitionManager.Get().IsInTransition();
        }
    }
}