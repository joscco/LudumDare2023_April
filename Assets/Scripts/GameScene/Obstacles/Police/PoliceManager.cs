using System.Collections.Generic;
using System.Linq;
using LevelDesign;
using UnityEngine;

namespace GameScene.Buildings
{
    public class PoliceManager : MonoBehaviour
    {
        [SerializeField] private PoliceBuilding policeStationPrefab;
        [SerializeField] private Grid grid;
        private List<PoliceBuilding> _buildings;

        private void Start()
        {
            _buildings = new();
        }

        public bool WatchesIndex(Vector2Int index)
        {
            return _buildings.Any(building => building.WatchesIndex(index));
        }

        public bool HasBuildingAt(Vector2Int index)
        {
            return _buildings.Any(building => building.GetIndex() == index);
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

        public PoliceBuilding GetBuildingAt(Vector2Int index)
        {
            return _buildings.First(building => building.GetIndex() == index);
        }

        public List<PoliceBuilding> GetBuildings()
        {
            return _buildings;
        }

        public void ShowAlarms()
        {
            foreach (var policeBuilding in _buildings)
            {
                policeBuilding.ShowAlarm();
            }
        }

        public void AddPoliceAt(PoliceBuilding policeInstance, Vector2Int policeDataPosition)
        {
            policeInstance.SetIndex(policeDataPosition);
            policeInstance.transform.position = grid.GetCellCenterWorld(indexWrap(policeInstance.GetIndex()));
            _buildings.Add(policeInstance);
        }

        public void InitPoliceDepartments(List<PoliceData> polices)
        {
            foreach (var policeData in polices)
            {
                var policeInstance = Instantiate(policeStationPrefab, transform);
                AddPoliceAt(policeInstance, policeData.Position);
            }
        }
    }
}