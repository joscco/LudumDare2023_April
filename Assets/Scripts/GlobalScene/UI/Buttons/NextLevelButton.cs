using Code.GameScene.UI;
using DG.Tweening;
using General;

namespace GameScene
{
    public class NextLevelButton : ScalingButton
    {
        private bool _fadeIn;
        public override void OnClick()
        {
            SceneManager.Get().LoadNextLevel();
            AudioManager.instance.PlayBlub();
            DOTween.Sequence()
                .Append(ScaleUpOnClick())
                .Append(ScaleDownAfterClick())
                .Play();
        }

        public override bool IsEnabled()
        {
            return !SceneManager.Get().IsInTransition();
        }
    }
}