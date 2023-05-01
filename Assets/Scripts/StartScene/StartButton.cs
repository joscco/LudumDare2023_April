using System;
using Code;
using Code.GameScene.UI;
using DG.Tweening;
using GameScene.UI;
using General;
using UnityEngine;

namespace StartScene
{
    public class StartButton : ScalingButton
    {
        public String levelChooserSceneName = "LevelChoosingScene";
        public SpriteRenderer heart;
        private bool _clickable;
        private Tween beatTween;

        public void Start()
        {
            _clickable = true;
            BeginBeating();
        }

        private void BeginBeating()
        {
            beatTween = DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(heart.transform.DOScale(1.1f, 0.1f)
                    .SetEase(Ease.OutExpo)
                    .SetLoops(1, LoopType.Yoyo))
                .AppendInterval(0.1f)
                .Append(heart.transform.DOScale(1.1f, 0.1f)
                    .SetEase(Ease.OutExpo)
                    .SetLoops(1, LoopType.Yoyo))
                .SetLoops(-1, LoopType.Restart);
        }

        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionTo(levelChooserSceneName);
            AudioManager.instance.PlayMusic();
            _clickable = false;
        }

        public override bool IsEnabled()
        {
            return _clickable && !OptionScreen.instance.IsVisible();
        }
    }
}
