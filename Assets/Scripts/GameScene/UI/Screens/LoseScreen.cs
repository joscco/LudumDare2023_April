using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class LoseScreen : MonoBehaviour
    {
        public Sprite mafiaReasonSprite;
        public Sprite policeReasonSprite;
        public TextMeshPro reasonText;
        public SpriteRenderer reasonSpriteRenderer;
        public TitleAnimation titleAnimation;
        
        private const int VERTICAL_OFFSET_WHEN_HIDDEN = 900;
        private bool _visible = false;

        private void Start()
        {
            Hide();
        }

        public void BlendIn(LoseReason reason)
        {
            UpdateReasonSprite(reason);
            UpdateReasonText(reason);
            _visible = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBack);
            titleAnimation.Hide();
            titleAnimation.FadeIn();
        }

        private void UpdateReasonSprite(LoseReason reason)
        {
            switch (reason)
            {
                case LoseReason.NoActionsLeft:
                    reasonText.text = "Oh no! You ran out of actions.";
                    break;
                case LoseReason.Mafia:
                    reasonText.text = "Oopsie... The mafia wasn't satisfied with you.";
                    break;
                case LoseReason.Police:
                    reasonText.text = "Hands Up! You got caught by the police.";
                    break;
            }
        }

        private void UpdateReasonText(LoseReason reason)
        {
            switch (reason)
            {
                case LoseReason.NoActionsLeft:
                    reasonSpriteRenderer.sprite = null;
                    break;
                case LoseReason.Mafia:
                    reasonSpriteRenderer.sprite = mafiaReasonSprite;
                    break;
                case LoseReason.Police:
                    reasonSpriteRenderer.sprite = policeReasonSprite;
                    break;
            }
        }

        public void BlendOut()
        {
            _visible = false;
            transform.DOMoveY(-VERTICAL_OFFSET_WHEN_HIDDEN, 0.5f).SetEase(Ease.InBack);
        }

        public void Hide()
        {
            _visible =false;
            transform.position = new Vector2(0, -VERTICAL_OFFSET_WHEN_HIDDEN);
        }

        public bool IsVisible()
        {
            return _visible;
        }
    }

    public enum LoseReason
    {
        NoActionsLeft, Mafia, Police
    }
}