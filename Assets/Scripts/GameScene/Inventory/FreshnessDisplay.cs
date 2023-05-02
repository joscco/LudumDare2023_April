using System;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Inventory
{
    public class FreshnessDisplay : MonoBehaviour
    {

        public SpriteRenderer smileyRenderer;
        public Sprite perfectSmiley;
        public Sprite goodSmiley;
        public Sprite badSmiley;
        
        public SpriteRenderer block1;
        public SpriteRenderer block2;
        public SpriteRenderer block3;

        private FreshnessLevel _status;
        private bool _shown;

        public void Hide()
        {
            _shown = false;
            transform.localScale = Vector3.zero;
        }

        public void BlendIn()
        {
            _shown = true;
            transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }

        public void BlendOut()
        {
            _shown = false;
            transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
        
        
        public void NextStatus()
        {
            if (_status != FreshnessLevel.INFINITE_PERFECT && _status != FreshnessLevel.BAD)
            {
                _status = NORMAL_STATUSES[(Array.IndexOf(NORMAL_STATUSES, _status) + 1) % NORMAL_STATUSES.Length];
            }
            
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            switch (_status)
            {
                case FreshnessLevel.INFINITE_PERFECT:
                    ScaleAndUpdate(perfectSmiley, 0, 0, 0);
                    break;
                case FreshnessLevel.JUST_TAKEN:
                case FreshnessLevel.PERFECT_0:
                    ScaleAndUpdate(perfectSmiley, 1, 1, 1);
                    break;
                case FreshnessLevel.PERFECT_1:
                    ScaleAndUpdate(perfectSmiley, 0, 1, 1);
                    break;
                case FreshnessLevel.PERFECT_2:
                    ScaleAndUpdate(perfectSmiley, 0, 0, 1);
                    break;
                case FreshnessLevel.GOOD_0:
                    ScaleAndUpdate(goodSmiley, 1, 1, 1);
                    break;
                case FreshnessLevel.GOOD_1:
                    ScaleAndUpdate(goodSmiley, 0, 1, 1);
                    break;
                case FreshnessLevel.GOOD_2:
                    ScaleAndUpdate(goodSmiley, 0, 0, 1);
                    break;
                case FreshnessLevel.BAD:
                    ScaleAndUpdate(badSmiley, 0, 0, 0);
                    break;
                    
            }
        }

        private void ScaleAndUpdate(Sprite smileySprite, float block1Scale, float block2Scale, float block3Scale)
        {
            smileyRenderer.sprite = smileySprite;
            smileyRenderer.transform.DOScale(1.05f, 0.1f).SetEase(Ease.InOutQuad).SetLoops(1, LoopType.Yoyo);
            block1.transform.DOScale(block1Scale, 0.2f).SetEase(Ease.InOutQuad);
            block2.transform.DOScale(block2Scale, 0.2f).SetEase(Ease.InOutQuad);
            block3.transform.DOScale(block3Scale, 0.2f).SetEase(Ease.InOutQuad);
        }

        public SimpleFreshnessLevel GetFreshness()
        {
            if (_status == FreshnessLevel.INFINITE_PERFECT || 
                _status == FreshnessLevel.JUST_TAKEN ||
                _status == FreshnessLevel.PERFECT_0 || 
                _status == FreshnessLevel.PERFECT_1 || 
                _status == FreshnessLevel.PERFECT_2)
            {
                return SimpleFreshnessLevel.PERFECT;
            }
            
            if (_status == FreshnessLevel.GOOD_0 ||
                _status == FreshnessLevel.GOOD_1 || 
                _status == FreshnessLevel.GOOD_2)
            {
                return SimpleFreshnessLevel.GOOD;
            }

            return SimpleFreshnessLevel.BAD;
        }
        
        FreshnessLevel[] NORMAL_STATUSES =
        {
            FreshnessLevel.JUST_TAKEN,
            FreshnessLevel.PERFECT_0,
            FreshnessLevel.PERFECT_1, 
            FreshnessLevel.PERFECT_2,
            FreshnessLevel.GOOD_0,
            FreshnessLevel.GOOD_1,
            FreshnessLevel.GOOD_2,
            FreshnessLevel.BAD
        };

        public void SetInfiniteFresh()
        {
            _status = FreshnessLevel.INFINITE_PERFECT;
        }

        public void SetRegularFresh()
        {
            _status = FreshnessLevel.JUST_TAKEN;
        }
    }

    public enum SimpleFreshnessLevel
    {
        PERFECT, GOOD, BAD
    }

    public enum FreshnessLevel
    {
        INFINITE_PERFECT,
        JUST_TAKEN,
        PERFECT_0,
        PERFECT_1, 
        PERFECT_2,
        GOOD_0,
        GOOD_1,
        GOOD_2,
        BAD
    }
}