using System.Collections.Generic;
using System.Linq;
using LevelDesign;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene.StreetMap
{
    public class LevelGrid: MonoBehaviour
    {
        [SerializeField] private Tilemap lawnMap;
        [SerializeField] private RuleTile lawnTile;
        [SerializeField] private Tilemap riverMap;
        [SerializeField] private RuleTile riverTile;
        [SerializeField] private Tilemap streetMap;
        [SerializeField] private RuleTile streetTile;
        [SerializeField] private Tilemap decorationMap;
        [SerializeField] private Tile horizontalBridgeTile;
        [SerializeField] private Tile verticalBridgeTile;

        public Grid grid;

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(IndexWrap(index));
        }
        public Vector2 GetCellSize()
        {
            return grid.cellSize;
        }

        public bool HasStreetAt(Vector2Int index)
        {
            return streetMap.HasTile(IndexWrap(index));
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public Vector2Int MoveOnStreetTowards(Vector2Int from, Vector2Int to)
        {
            var streetIndices = new List<Vector2Int>();

            foreach (var pos in streetMap.cellBounds.allPositionsWithin)
            {   
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (streetMap.HasTile(localPlace))
                {
                    streetIndices.Add(new Vector2Int(localPlace.x, localPlace.y));
                }
            }

            if (!streetIndices.Contains(from))
            {
                streetIndices.Add(from);
            }
            
            if (!streetIndices.Contains(to))
            {
                streetIndices.Add(to);
            }
           
            var shortestPath = FindBestPath(streetIndices, from, to);

            if (shortestPath.Count == 0)
            {
                return from;
            }
            
            return shortestPath[0];
        }

        private List<Vector2Int> FindBestPath(List<Vector2Int> indices, Vector2Int from, Vector2Int to)
        {
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, int> stepsSoFar = new Dictionary<Vector2Int, int>();
            
            List<Vector2Int> path = new List<Vector2Int>();

            List<Vector2Int> frontier = new List<Vector2Int>();
            frontier.Add(from);
            cameFrom.Add(from, from);
            stepsSoFar.Add(from, 0);

            Vector2Int current = from;
            while (frontier.Count > 0)
            {
                current = frontier[0];
                frontier.RemoveAt(0);
                
                if (current == to)
                {
                    break;
                }

                foreach (var neighbor in FindNeighbors(indices, current))
                {
                    int newCost = stepsSoFar[current] + 1;
                    if (!stepsSoFar.ContainsKey(neighbor) || newCost < stepsSoFar[neighbor])
                    {
                        stepsSoFar[neighbor] = newCost;
                        cameFrom[neighbor] = current;
                        frontier.Add(neighbor);
                    }
                }
            }
            
            while (current != from)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();

            return path;
        }
        
        private List<Vector2Int> FindNeighbors(List<Vector2Int> allowedIndices, Vector2Int index)
        {
            return new List<Vector2Int>()
            {
                new(index.x - 1, index.y),
                new(index.x + 1, index.y),
                new(index.x, index.y - 1),
                new(index.x, index.y + 1),
            }
                .Where(neighborIndex => allowedIndices.Contains(neighborIndex))
                .ToList();
        }

        public void InitLandscape(Vector2Int levelDimensions, List<RiverData> rivers, List<BridgeData> bridges, List<StreetData> streets)
        {
            lawnMap.ClearAllTiles();
            riverMap.ClearAllTiles();
            streetMap.ClearAllTiles();
            decorationMap.ClearAllTiles();
            
            // Center grid position
            transform.position = 55 * Vector2.down - 0.5f * 100 * new Vector2(levelDimensions.x, levelDimensions.y);

            for (int x = 0; x < levelDimensions.x; x++)
            {
                for (int y = 0; y < levelDimensions.y; y++)
                {
                    lawnMap.SetTile(new Vector3Int(x, y, 0), lawnTile);
                }
            }

            foreach (var river in rivers)
            {
                riverMap.SetTile(new Vector3Int(river.Position.x, river.Position.y), riverTile);
            }

            foreach (var street in streets)
            {
                streetMap.SetTile(new Vector3Int(street.Position.x, street.Position.y), streetTile);
            }

            foreach (var bridge in bridges)
            {
                switch (bridge.Type)
                {
                    case BridgeType.Horizontal:
                        decorationMap.SetTile(new Vector3Int(bridge.Position.x, bridge.Position.y), horizontalBridgeTile);
                        break;

                    case BridgeType.Vertical:
                        decorationMap.SetTile(new Vector3Int(bridge.Position.x, bridge.Position.y), verticalBridgeTile);
                        break;
                }
            }
        }
    }
    
    
}