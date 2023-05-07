using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.BuildingMap
{
    public class TrafficLightManager : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        private List<TrafficLight> _trafficLights;

        private void Start()
        {
            _trafficLights = new();
        }

        public bool HasLightAt(Vector2Int index)
        {
            return _trafficLights.Any(light => light.GetIndex() == index);
        }

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(indexWrap(index));
        }

        public Vector2 GetCellSize()
        {
            return grid.cellSize;
        }

        public Vector3Int indexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public Vector2Int indexUnwrap(Vector3Int index)
        {
            return new Vector2Int(index.x, index.y);
        }

        public TrafficLight GetTrafficLightAt(Vector2Int index)
        {
            return _trafficLights.First(light => light.GetIndex() == index);
        }

        public bool CanEnter(Vector2Int currentIndex)
        {
            foreach (var trafficLight in _trafficLights)
            {
                if (trafficLight.GetIndex() == currentIndex)
                {
                    return trafficLight.IsOnGo();
                }
            }

            // No traffic Light Found
            return true;
        }

        public void NextStatus()
        {
            foreach (var trafficLight in _trafficLights)
            {
                trafficLight.NextStatus();
            }
        }

        public void AddLightAt(TrafficLight trafficLightInstance, Vector2Int trafficLightPosition)
        {
            trafficLightInstance.SetIndex(trafficLightPosition);
            trafficLightInstance.transform.position =
                grid.GetCellCenterWorld(indexWrap(trafficLightInstance.GetIndex()));
        }
    }
}