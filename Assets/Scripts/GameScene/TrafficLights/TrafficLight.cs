using System;
using System.Linq;
using UnityEngine;

namespace GameScene.BuildingMap
{
    public class TrafficLight : MonoBehaviour
    {
        [SerializeField] private TrafficLightStatus _status;
        [SerializeField] private Sprite GoSprite;
        [SerializeField] private Sprite StopSprite;
        [SerializeField] private SpriteRenderer SpriteRenderer;

        private void Start()
        {
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            SpriteRenderer.sprite = IsOnGo() ? GoSprite : StopSprite;
        }

        // Will be set at start by grid
        private Vector2Int _index;

        public void SetIndex(Vector2Int index)
        {
            _index = index;
        }

        public Vector2Int GetIndex()
        {
            return _index;
        }

        public void NextStatus()
        {
            _status = STATUSES[(Array.IndexOf(STATUSES, _status) + 1) % STATUSES.Length];
            UpdateSprite();
        }

        public bool IsOnGo()
        {
            return GO_STATUSES.Contains(_status);
        }

        private static readonly TrafficLightStatus[] GO_STATUSES =
            { TrafficLightStatus.GO_FIRST, TrafficLightStatus.GO_SECOND };

        private static readonly TrafficLightStatus[] STATUSES =
        {
            TrafficLightStatus.GO_FIRST,
            TrafficLightStatus.GO_SECOND,
            TrafficLightStatus.STOP_FIRST,
            TrafficLightStatus.STOP_SECOND
        };
    }

    public enum TrafficLightStatus
    {
        GO_FIRST,
        GO_SECOND,
        STOP_FIRST,
        STOP_SECOND
    }
}