using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class DialogManager : MonoBehaviour
    {
        public SpriteRenderer bubble;
        public SpriteRenderer mafiaGuy;
        public TextMeshPro textField;
        
        private List<DialogNode> dialogTexts;

        private const int BubbleOffsetWhenHidden = -1000;
        private const int BubbleOffsetWhenShown = -540;
        private const int MafiaguyOffsetWhenHidden = -1000;
        private const int MafiaguyOffsetWhenShown = -350;
        private bool shown;

        private Tween _bubbleMoveTween;
        private Tween _mafiaGuyMoveTween;
        private Tween _textFadeTween;

        private void Start()
        {
            bubble.transform.position = new Vector2(bubble.transform.position.x, BubbleOffsetWhenHidden);
            mafiaGuy.transform.position = new Vector2(mafiaGuy.transform.position.x, MafiaguyOffsetWhenHidden);
            textField.text = "";
            dialogTexts = new List<DialogNode>();
        }

        private void Update()
        {
            if (dialogTexts.Count > 0 && !shown)
            {
                BlendInWithOldestText();
            } else if (dialogTexts.Count == 0 && shown)
            {
                BlendOut();
            }
        }

        private void BlendOut()
        {
            BlendOutMafiaGuy();
            BlendOutBubble();
        }

        private Tween BlendOutBubble()
        {
            shown = false;
            _bubbleMoveTween?.Kill();
            _bubbleMoveTween = bubble.transform.DOMoveY(BubbleOffsetWhenHidden, 0.3f).SetEase(Ease.InOutQuad);
            return _bubbleMoveTween;
        }
        
        private Tween BlendInBubble()
        {
            shown = true;
            _bubbleMoveTween?.Kill();
            _bubbleMoveTween = bubble.transform.DOMoveY(BubbleOffsetWhenShown, 0.3f).SetEase(Ease.InOutQuad);
            return _bubbleMoveTween;
        }

        private Tween BlendOutMafiaGuy()
        {
            _mafiaGuyMoveTween?.Kill();
            _mafiaGuyMoveTween = mafiaGuy.transform.DOMoveY(MafiaguyOffsetWhenHidden, 0.3f).SetEase(Ease.InOutQuad);
            return _mafiaGuyMoveTween;
        }
        
        private Tween BlendInMafiaGuy()
        {
            _mafiaGuyMoveTween?.Kill();
            _mafiaGuyMoveTween = mafiaGuy.transform.DOMoveY(MafiaguyOffsetWhenShown, 0.3f).SetEase(Ease.InOutQuad);
            return _mafiaGuyMoveTween;
        }
        
        private Tween BlendInText()
        {
            _textFadeTween?.Kill();
            _textFadeTween = textField.DOFade(1, 0.2f).SetEase(Ease.InOutQuad);
            return _textFadeTween;
        }

        private Tween BlendOutText()
        {
            _textFadeTween?.Kill();
            _textFadeTween = textField.DOFade(0, 0.2f).SetEase(Ease.InOutQuad);
            return _textFadeTween;
        }

        private void BlendInWithOldestText()
        {
            if (null != dialogTexts[0])
            {
                _bubbleMoveTween?.Kill();
                _mafiaGuyMoveTween?.Kill();
                var nextNode = dialogTexts[0];

                if (shown)
                {
                    DOTween.Sequence()
                        .Append(nextNode.showMafiaGuy ? BlendInMafiaGuy() : BlendOutMafiaGuy())
                        .Append(BlendOutText())
                        .AppendCallback(() => textField.text = nextNode.text)
                        .Append(BlendInText());
                }
                else
                {
                    textField.text = nextNode.text;
                    DOTween.Sequence()
                        .Append(BlendInBubble())
                        .Append(nextNode.showMafiaGuy ? BlendInMafiaGuy() : BlendOutMafiaGuy());
                }
            }
        }

        public void SkipCurrentDialog()
        {
            if (dialogTexts.Count() > 0)
            {
                dialogTexts.RemoveAt(0);
                if (dialogTexts.Count > 0)
                {
                    BlendInWithOldestText();
                }
            }
        }

        public void ShowYouCannotHoldFoodAndIllegalStuff()
        {
            AddDialog("You cannot deliver food and illegal goods at the same time!", false);
        }

        public void AddDialog(String text, bool showMafiaGuy)
        {
            AddDialog(new List<string>(){text}, showMafiaGuy);
        }

        public void AddDialog(List<String> texts, bool showMafiaGuy)
        {
            dialogTexts.AddRange(texts.Select(text => new DialogNode(text, showMafiaGuy)));
        }

        public class DialogNode
        {
            public String text;
            public bool showMafiaGuy;

            public DialogNode(String text, bool showMafiaGuy)
            {
                this.text = text;
                this.showMafiaGuy = showMafiaGuy;
            }
        }
    }
}