using System.Collections.Generic;
using System.Linq;
using GameScene.BuildingMap;
using LevelDesign;
using UnityEngine;

namespace GameScene.Obstacles.TrafficLights
{
    public class TrafficLightManager : MonoBehaviour
    {
        [SerializeField] private TrafficLight trafficLightPrefab;
        [SerializeField] private Grid grid;
        private List<TrafficLight> _trafficLights;

        private void Start()
        {
            _trafficLights = new();
        }

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(IndexWrap(index));
        }

        public Vector2 GetCellSize()
        {
            return grid.cellSize;
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }
        
        public TrafficLight GetTrafficLightAt(Vector2Int index)
        {
            return _trafficLights.First(trafficLight => trafficLight.GetIndex() == index);
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
            trafficLightInstance.transform.position = IndexToPosition(trafficLightInstance.GetIndex());
            _trafficLights.Add(trafficLightInstance);
        }

        public void InitTrafficLights(List<TrafficLightData> trafficLights)
        {
            foreach (var trafficLight in trafficLights)
            {
                var trafficLightInstance = Instantiate(trafficLightPrefab, transform);
                trafficLightInstance.status = trafficLight.StartStatus; 
                AddLightAt(trafficLightInstance, trafficLight.Position);
            }
        }
    }
}