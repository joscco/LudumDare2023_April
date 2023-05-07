using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Dogs
{
    public class Dog : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer warnSymbol;
        [SerializeField] private SpriteRenderer dogRenderer;
        public Sprite standingDog;
        public Sprite runningDog;
        private Vector2Int _index;
        private Vector2Int _startIndex;
        private DogStatus _status = DogStatus.Home;
        private Tween _moveTween;
        private Tween _scaleTween;

        private void Start()
        {
            warnSymbol.transform.localScale = Vector3.zero;
            StartShaking();
        }
        
        private void StartShaking()
        {
            _scaleTween = dogRenderer.transform.DOScale(new Vector3(0.9f, 1.03f, 0.8f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void ShowAlarm()
        {
            warnSymbol.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }
        
        public void UnshowAlarm()
        {
            warnSymbol.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack);
        }

        public void SetIndex(Vector2Int indexUnwrap)
        {
            _index = indexUnwrap;
        }

        public Vector2Int GetIndex()
        {
            return _index;
        }

        public void SetStartIndex(Vector2Int startIndex)
        {
            _startIndex = startIndex;
        }

        public Vector2Int GetStartIndex()
        {
            return _startIndex;
        }

        public bool WatchesIndex(Vector2Int index)
        {
            return Math.Abs(index.x - _index.x) <= 2 && Math.Abs(index.y - _index.y) <= 2;
        }

        public DogStatus GetStatus()
        {
            return _status;
        }

        public void UpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            if (newIndex.x < _index.x)
            {
                dogRenderer.flipX = true;
            }
            else
            {
                dogRenderer.flipX = false;
            }
            
            _index = newIndex;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(newPos, 0.15f)
                .SetEase(Ease.InOutCirc);

            if (_status == DogStatus.Homecoming && newIndex == _startIndex)
            {
                _status = DogStatus.Home;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            switch (_status)
            {
                case DogStatus.Following:
                case DogStatus.Homecoming:
                    dogRenderer.sprite = runningDog;
                    break;
                case DogStatus.Home:
                    dogRenderer.sprite = standingDog;
                    break;
            }
        }

        public void Cancel()
        {
            _status = DogStatus.Canceled;
            BlendOut();
        }

        private void BlendOut()
        {
            _scaleTween?.Kill();
            transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
        }

        public void GoHome()
        {
            _status = DogStatus.Homecoming;
            UnshowAlarm();
        }

        public void StartFollowing()
        {
            _status = DogStatus.Following;
            UpdateSprite();
            ShowAlarm();
        }
    }

    public enum DogStatus
    {
        Home,
        Homecoming,
        Following,
        Canceled
    }
}