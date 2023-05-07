using Code.GameScene.UI;
using GameScene.UI;

namespace GameScene
{
    public class OptionButton : ScalingButton
    {
        public OptionScreen OptionScreen;
        
        public override void OnClick()
        {
            OptionScreen.Toggle();
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}