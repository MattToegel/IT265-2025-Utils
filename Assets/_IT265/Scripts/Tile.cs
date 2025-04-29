using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [DisallowMultipleComponent]
    public class Tile : MonoBehaviour
    {
        public static Dictionary<(int, int), Tile> allTiles = new();

        [Header("Tile Info")]
        public int gridX;
        public int gridY;

        [SerializeField] private Vector3 offset = Vector3.zero;

        [SerializeField]
        private List<Direction> allowedDirections = new();

        [System.Serializable]
        public class NeighborLink
        {
            public Direction direction;
            public Tile tile;
        }

        [Header("Neighbor Mapping (Inspector Only)")]
        [SerializeField]
        private List<NeighborLink> neighborLinks = new();

        public Dictionary<Direction, Tile> neighbors = new();
        public List<NeighborLink> GetAvailableNeighbors()
        {
            return neighborLinks;
        }
        public void Initialize(int x, int y, HashSet<Direction> directions)
        {
            gridX = x;
            gridY = y;
            allowedDirections = new List<Direction>(directions);
        }

        public void ResolveNeighbors(int width, int height, bool wrapHorizontal, bool wrapVertical)
        {
            neighbors.Clear();
            neighborLinks.Clear();

            foreach (var dir in allowedDirections)
            {
                (int dx, int dy) = dir switch
                {
                    Direction.Left => (-1, 0),
                    Direction.Right => (1, 0),
                    Direction.Up => (0, -1),
                    Direction.Down => (0, 1),
                    _ => (0, 0)
                };

                int targetX = gridX + dx;
                int targetY = gridY + dy;

                if (wrapHorizontal)
                {
                    if (targetX < 0) targetX = width - 1;
                    if (targetX >= width) targetX = 0;
                }

                if (wrapVertical)
                {
                    if (targetY < 0) targetY = height - 1;
                    if (targetY >= height) targetY = 0;
                }

                if (allTiles.TryGetValue((targetX, targetY), out Tile neighbor))
                {
                    neighbors[dir] = neighbor;
                    neighborLinks.Add(new NeighborLink { direction = dir, tile = neighbor });
                }
                else
                {
                    neighborLinks.Add(new NeighborLink { direction = dir, tile = null });
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (neighbors == null || neighbors.Count == 0)
                return;

            Vector3 offset = AdvancedLevelBuilder.ArrowOffset;

            foreach (var kvp in neighbors)
            {
                Tile neighbor = kvp.Value;
                if (neighbor == null) continue;

                Vector3 start = transform.position + offset;
                Vector3 end = neighbor.transform.position + offset;

                Gizmos.color = Color.yellow;
                DrawArrow(start, end, 0.2f, 0.1f);
            }
        }

        private void DrawArrow(Vector3 start, Vector3 end, float headLength, float headWidth)
        {
            Gizmos.DrawLine(start, end);

            Vector3 direction = (end - start).normalized;
            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized; // Always use global up

            Vector3 arrowLeft = end - direction * headLength + right * headWidth;
            Vector3 arrowRight = end - direction * headLength - right * headWidth;

            Gizmos.DrawLine(end, arrowLeft);
            Gizmos.DrawLine(end, arrowRight);
        }

#endif
    }
}
