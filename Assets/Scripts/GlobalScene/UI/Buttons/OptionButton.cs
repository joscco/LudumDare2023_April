using Code.GameScene.UI;
using DG.Tweening;
using GameScene.UI;
using General;

namespace GameScene
{
    public class OptionButton : ScalingButton
    {
        public OptionScreen OptionScreen;
        
        public override void OnClick()
        {
            OptionScreen.Toggle();
            AudioManager.instance.PlayBlub();

            DOTween.Sequence()
                .Append(ScaleUpOnClick())
                .Append(ScaleDownAfterClick())
                .Play();
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}