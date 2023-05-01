using Code.GameScene.UI;
using GameScene.UI;

namespace GameScene
{
    public class OptionButton : ScalingButton
    {
        public OptionScreen OptionScreen;
        
        public override void OnClick()
        {
            if (OptionScreen.IsVisible())
            {
                OptionScreen.BlendOut();
            }
            else
            {
                OptionScreen.BlendIn();
            }
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}