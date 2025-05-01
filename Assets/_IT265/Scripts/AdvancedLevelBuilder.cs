using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Demo
{
    public class AdvancedLevelBuilder : MonoBehaviour
    {
        [Header("CSV Grid Input")]
        [TextArea(5, 20)]
        public string csvGrid;

        [Header("Symbol to Prefab Mapping")]
        public List<SymbolMapping> symbolMappings;

        [Header("Grid Settings")]
        public float spacing = 1f;
        public GridPlane plane = GridPlane.XY;
        public Vector3 prefabPositionOffset = Vector3.zero;
        public Vector3 prefabRotationEuler = Vector3.zero;

        [Header("Wrapping Settings")]
        public bool allowHorizontalWrapping = true;
        public bool allowVerticalWrapping = false;

        [Header("Gizmo Settings")]
        public Vector3 arrowOffset = new Vector3(0, 0.25f, 0);

        private const string GRID_CONTAINER_NAME = "GeneratedGrid";

        public static Vector3 ArrowOffset { get; private set; }

        private void Awake()
        {
            ArrowOffset = arrowOffset;
        }

        private void OnValidate()
        {
            ArrowOffset = arrowOffset;
        }

        [ContextMenu("Generate Grid")]
        public void GenerateGrid()
        {
#if UNITY_EDITOR
            Tile.allTiles.Clear();

            Transform existingGrid = transform.Find(GRID_CONTAINER_NAME);
            if (existingGrid != null)
            {
                bool overwrite = EditorUtility.DisplayDialog(
                    "Grid Already Exists",
                    "A generated grid already exists. What would you like to do?",
                    "Overwrite",
                    "Make a Copy"
                );

                if (overwrite)
                {
                    DestroyImmediate(existingGrid.gameObject);
                    GenerateGridInternal(GRID_CONTAINER_NAME);
                }
                else
                {
                    int suffix = 1;
                    string newName;
                    do
                    {
                        newName = $"{GRID_CONTAINER_NAME} ({suffix})";
                        suffix++;
                    } while (transform.Find(newName) != null);

                    GenerateGridInternal(newName);
                }
            }
            else
            {
                GenerateGridInternal(GRID_CONTAINER_NAME);
            }

            // Ensure latest offset even after generation
            ArrowOffset = arrowOffset;
#endif
        }

        private void GenerateGridInternal(string containerName)
        {
            Dictionary<string, GameObject> symbolMap = new();
            foreach (var mapping in symbolMappings)
            {
                if (!string.IsNullOrEmpty(mapping.symbol) && mapping.prefab != null)
                    symbolMap[mapping.symbol] = mapping.prefab;
            }

            string[] rows = csvGrid.Split('\n');
            int height = rows.Length;
            int width = 0;

            GameObject container = new GameObject(containerName);
            container.transform.parent = this.transform;

            for (int y = 0; y < height; y++)
            {
                string row = rows[y].Trim();
                if (string.IsNullOrWhiteSpace(row)) continue;

                string[] symbols = row.Split(',');
                if (symbols.Length > width)
                    width = symbols.Length;

                for (int x = 0; x < symbols.Length; x++)
                {
                    string cell = symbols[x].Trim();
                    if (string.IsNullOrEmpty(cell)) continue;

                    string symbol = GetSymbol(cell);
                    HashSet<Direction> directions = GetDirections(cell);

                    if (symbolMap.TryGetValue(symbol, out GameObject prefab))
                    {
                        Vector3 position = CalculatePosition(x, y);
                        GameObject obj = Instantiate(prefab, position, Quaternion.Euler(prefabRotationEuler), container.transform);
                        obj.transform.localPosition += prefabPositionOffset;
                        obj.name = $"{symbol}_({x},{y})";

                        Tile tile = obj.GetComponent<Tile>();
                        if (tile == null)
                            tile = obj.AddComponent<Tile>();

                        tile.Initialize(x, y, directions);
                        Tile.allTiles[(x, y)] = tile;
                    }
                }
            }

            foreach (var tile in Tile.allTiles.Values)
            {
                tile.ResolveNeighbors(width, height, allowHorizontalWrapping, allowVerticalWrapping);
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        private string GetSymbol(string cell)
        {
            int index = cell.IndexOfAny(new[] { '<', '>', '^', '_' });
            return index >= 0 ? cell.Substring(0, index) : cell;
        }

        private HashSet<Direction> GetDirections(string cell)
        {
            HashSet<Direction> dirs = new();
            foreach (char c in cell)
            {
                switch (c)
                {
                    case '<': dirs.Add(Direction.Left); break;
                    case '>': dirs.Add(Direction.Right); break;
                    case '^': dirs.Add(Direction.Up); break;
                    case '_': dirs.Add(Direction.Down); break;
                }
            }
            return dirs;
        }

        private Vector3 CalculatePosition(int x, int y)
        {
            float sx = x * spacing;
            float sy = y * spacing;

            return plane switch
            {
                GridPlane.XY => new Vector3(sx, -sy, 0),
                GridPlane.XZ => new Vector3(sx, 0, sy),
                GridPlane.YZ => new Vector3(0, sx, sy),
                _ => Vector3.zero
            };
        }
    }

    public enum GridPlane
    {
        XY,
        XZ,
        YZ
    }

    [System.Serializable]
    public class SymbolMapping
    {
        public string symbol;
        public GameObject prefab;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
