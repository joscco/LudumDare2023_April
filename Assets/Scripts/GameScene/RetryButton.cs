using Code.GameScene.UI;

namespace GameScene
{
    public class RetryButton : ScalingButton
    {
        public TitleAnimation Animation;
        private bool _fadeIn;
        public override void OnClick()
        {
            if (_fadeIn)
            {
                Animation.FadeOut();
            }
            else
            {
                Animation.FadeIn();
            }

            _fadeIn = !_fadeIn;
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}