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

        public void Start()
        {
            _clickable = true;
            BeginBeating();
        }

        private void BeginBeating()
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .Append(heart.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutExpo))
                .Append(heart.transform.DOScale(1f, 0.1f).SetEase(Ease.OutExpo))
                .AppendInterval(0.1f)
                .Append(heart.transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutExpo))
                .Append(heart.transform.DOScale(1f, 0.1f).SetEase(Ease.OutExpo))
                .SetLoops(-1, LoopType.Restart);
        }

        public override void OnClick()
        {
            SceneManager.Get().TransitionTo(levelChooserSceneName);
            AudioManager.instance.PlayMusic();
            AudioManager.instance.PlayBlub();
            DOTween.Sequence()
                .Append(ScaleUpOnClick())
                .Append(ScaleDownAfterClick())
                .Play();
            _clickable = false;
        }

        public override bool IsEnabled()
        {
            return _clickable && !OptionScreen.instance.IsVisible();
        }
    }
}
