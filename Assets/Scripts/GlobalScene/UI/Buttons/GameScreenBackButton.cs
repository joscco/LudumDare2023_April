using Code.GameScene.UI;
using DG.Tweening;
using General;

namespace GameScene.UI
{
    public class GameScreenBackButton : ScalingButton
    {
        public string sceneName = "LevelChooserScene";
        
        public override void OnClick()
        {
            SceneManager.Get().TransitionTo(sceneName);
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