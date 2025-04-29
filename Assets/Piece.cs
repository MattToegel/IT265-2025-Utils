using UnityEngine;

namespace Demo
{
    public class Piece : MonoBehaviour
    {
        public Tile currentTile;

        public void MoveSteps(int steps)
        {
            Tile tile = currentTile;
            // TODO fix issue where only moves 1 step regardless of number of steps passed
            // likely a logic order issue due to rushing this
            for (int i = 0; i < steps; i++)
            {
                var neighbors = currentTile.GetAvailableNeighbors();
                Debug.Log($"Moving step {i}");
                if (neighbors.Count > 0 && i < neighbors.Count)
                {
                    // Always pick the first neighbor for now (later, more intelligent logic)
                    tile = neighbors[i].tile;
                    if (!tile)
                    {
                        Debug.LogWarning("No tile found while moving!");
                        return;
                    }

                    currentTile = tile;
                    transform.position = tile.transform.position + Vector3.up * 0.5f; // Raise piece slightly
                }
                else
                {
                    Debug.LogWarning("No neighbors to move to!");
                    return;
                }
            }


            OnLandedOnTile();
        }

        private void OnLandedOnTile()
        {
            Debug.Log($"Landed on {currentTile.name}.");
            // Future: Invoke tile effects here
        }
    }
}