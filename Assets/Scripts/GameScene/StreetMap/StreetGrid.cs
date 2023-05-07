using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene.PlayerControl
{
    public class StreetGrid: MonoBehaviour
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
                .Where(index => allowedIndices.Contains(index))
                .ToList();
        }
    }
    
    
}