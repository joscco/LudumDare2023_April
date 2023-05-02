using System;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Buildings
{
    public class PoliceBuilding : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer warnSymbol;
        private Vector2Int _index;

        private void Start()
        {
            warnSymbol.transform.localScale = Vector3.zero;
        }

        public void ShowAlarm()
        {
            warnSymbol.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        }
        
        public void SetIndex(Vector2Int indexUnwrap)
        {
            _index = indexUnwrap;
        }
        
        public Vector2Int GetIndex()
        {
            return _index;
        }

        public bool WatchesIndex(Vector2Int index)
        {
            return Math.Abs(index.x - _index.x) <= 2 && Math.Abs(index.y - _index.y) <= 2;
        }
    }
}