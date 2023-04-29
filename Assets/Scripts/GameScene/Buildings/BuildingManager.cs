using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.BuildingMap
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        private Building[] _buildings;

        private void Start()
        {
            _buildings = GetComponentsInChildren<Building>();
            foreach (var building in _buildings)
            {
                building.SetIndex(indexUnwrap(grid.WorldToCell(building.transform.position)));
                building.transform.position = grid.GetCellCenterWorld(indexWrap(building.GetIndex()));
            }
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

        public Building GetBuildingAt(Vector2Int index)
        {
            return _buildings.First(building => building.GetIndex() == index);
        }
    }
}