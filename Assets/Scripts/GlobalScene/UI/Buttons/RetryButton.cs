using Code.GameScene.UI;
using DG.Tweening;
using GameScene.UI;
using General;

namespace GameScene
{
    public class RetryButton : ScalingButton
    {
        public override void OnClick()
        {
            SceneManager.Get().ReloadCurrentLevel();
            OptionScreen.instance.BlendOut();
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