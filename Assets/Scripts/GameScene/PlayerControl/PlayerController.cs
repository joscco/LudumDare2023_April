using DG.Tweening;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        
        public SpriteRenderer spriteRenderer;

        private Vector2Int _currentIndex;
        private Tween _moveTween;
        private Tween _scaleTween;

        private void Start()
        {
            StartShaking();
        }

        private void StartShaking()
        {
            _scaleTween = spriteRenderer.transform.DOScale(new Vector3(0.9f, 1.03f, 0.8f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void InstantUpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            _currentIndex = newIndex;
            _moveTween?.Kill();
            transform.position = newPos;
        }

        public void UpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            if (newIndex.x < _currentIndex.x)
            {
                spriteRenderer.flipX = true;
            }
            else if (newIndex.x > _currentIndex.x)
            {
                spriteRenderer.flipX = false;
            }
            _currentIndex = newIndex;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(newPos, 0.15f)
                .SetEase(Ease.InOutCirc);
        }

        public Vector2Int GetIndex()
        {
            return _currentIndex;
        }
    }
}