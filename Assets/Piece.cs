using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
    [DisallowMultipleComponent]
    public class Piece : MonoBehaviour {
        public static List<Piece> allPieces = new();

        public Tile currentTile;
        public int range = 2;
        public int health = 3;
        public int damage = 1;
        public int attack = 1;
        public int defense = 1;

        private void Awake() {
            allPieces.Add(this);
        }

        private void OnDestroy() {
            allPieces.Remove(this);
        }

        private IEnumerator DoMove(int steps) {
            for (int i = 0; i < steps; i++) {
                var neighbors = currentTile.GetAvailableNeighbors();
                if (neighbors.Count == 0) {
                    Debug.LogWarning("No neighbors to move to!");
                    yield break;
                }

                Tile target = neighbors[Random.Range(0, neighbors.Count)].tile;

                if (!target) {
                    Debug.LogWarning("No valid target tile during movement!");
                    yield break;
                }

                Debug.Log($"Moving step {i} to {target.name}");

                currentTile = target;
                transform.position = target.transform.position + Vector3.up * 0.5f;
                yield return new WaitForSeconds(0.25f);
            }

            OnLandedOnTile();
        }

        public void MoveSteps(int steps) {
            StartCoroutine(DoMove(steps));
        }

        private void OnLandedOnTile() {
            Debug.Log($"Landed on {currentTile.name}.");
            // Auto attack sample

            // Attempt to find targets
            var nearbyTiles = Tile.GetTilesInRangeOrdered(currentTile, range);
            if (nearbyTiles.Count == 0) {
                Debug.Log("No tiles within range");
            }
            foreach (var (tile, dist) in nearbyTiles) {
                var enemies = FindPiecesOnTile(tile, this);
                if (enemies.Count == 0) {
                    Debug.Log("No enemies found");
                }
                foreach (var enemy in enemies) {
                    Debug.Log($"Found enemy {enemy.name} on {tile.name} within range {dist}.");
                    int enemyDefense = Random.Range(0, enemy.defense+1);
                    int myAttack = Random.Range(0, attack + 1);
                    bool hit = myAttack > enemyDefense;
                    if(hit) {
                        enemy.health -= damage;
                    }
                    string hitMissMessage = hit ? $"hit for {damage}" : "missed";
                    Debug.Log($"Rolled attack {myAttack} vs defense {enemyDefense} {hitMissMessage}");
                }
            }
        }

        public static List<Piece> FindPiecesOnTile(Tile targetTile, Piece excludePiece = null) {
            List<Piece> found = new();
            foreach (var piece in allPieces) {
                if (piece != null && piece.currentTile.gridX == targetTile.gridX && piece.currentTile.gridY == targetTile.gridY) {
                    if (excludePiece != null && piece == excludePiece)
                        continue;

                    found.Add(piece);
                }
            }
            return found;
        }

    }
}
