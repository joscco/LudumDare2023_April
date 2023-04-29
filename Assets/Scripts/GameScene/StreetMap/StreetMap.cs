using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene.PlayerControl
{
    public class StreetMap: MonoBehaviour
    {
        public Tilemap streetMap;
        public Grid grid;

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(indexWrap(index));
        }
        public Vector2 GetCellSize()
        {
            return grid.cellSize;
        }

        public bool HasStreetAt(Vector2Int index)
        {
            return streetMap.HasTile(indexWrap(index));
        }

        public Vector3Int indexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }
        
    }
}