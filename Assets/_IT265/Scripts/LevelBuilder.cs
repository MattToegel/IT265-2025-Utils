using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
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

    private const string GRID_CONTAINER_NAME = "GeneratedGrid";

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
#if UNITY_EDITOR
        Transform existingGrid = transform.Find(GRID_CONTAINER_NAME);
        if (existingGrid != null)
        {
            bool shouldOverwrite = UnityEditor.EditorUtility.DisplayDialog(
                "Grid Already Exists",
                "A generated grid already exists. What would you like to do?",
                "Overwrite",
                "Make a Copy"
            );

            if (shouldOverwrite)
            {
                DestroyImmediate(existingGrid.gameObject);
            }
            else
            {
                int suffix = 1;
                string baseName = GRID_CONTAINER_NAME;
                string newName = $"{baseName} ({suffix})";
                while (transform.Find(newName) != null)
                {
                    suffix++;
                    newName = $"{baseName} ({suffix})";
                }
                GenerateGridInternal(newName);
                return;
            }
        }

        GenerateGridInternal(GRID_CONTAINER_NAME);
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
        GameObject container = new GameObject(containerName);
        container.transform.parent = this.transform;

        for (int y = 0; y < rows.Length; y++)
        {
            string row = rows[y].Trim();
            if (string.IsNullOrWhiteSpace(row)) continue;

            string[] symbols = row.Split(',');
            for (int x = 0; x < symbols.Length; x++)
            {
                string symbol = symbols[x].Trim();
                if (symbolMap.TryGetValue(symbol, out var prefab))
                {
                    Vector3 position = GetPosition(x, y);
                    GameObject obj = Instantiate(prefab, position, Quaternion.Euler(prefabRotationEuler), container.transform);
                    obj.transform.localPosition += prefabPositionOffset;
                    obj.name = $"{symbol}_({x},{y})";
                }
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    private Vector3 GetPosition(int x, int y)
    {
        float sx = x * spacing;
        float sy = y * spacing;

        return plane switch
        {
            GridPlane.XY => new Vector3(sx, -sy, 0),
            GridPlane.XZ => new Vector3(sx, 0, -sy),
            GridPlane.YZ => new Vector3(0, sx, -sy),
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
