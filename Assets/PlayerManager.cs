using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        public List<Player> players = new();
        private int currentPlayerIndex = 0;

        public GameObject playerPrefab;
        public Transform piecesParent; // where spawned pieces go
        public Piece startingPiecePrefab; // piece to assign each player

        private void Awake()
        {
            Instance = this;
        }

        public void CreatePlayers(int count)
        {
            players.Clear();

            for (int i = 0; i < count; i++)
            {
                GameObject playerObj = Instantiate(playerPrefab);
                playerObj.name = $"Player {i + 1}";
                Player player = playerObj.GetComponent<Player>();

                Piece piece = Instantiate(startingPiecePrefab, piecesParent);
                player.controlledPiece = piece;
                // TODO: Assign starting tile like 0,0 later here
                piece.currentTile = Tile.GetTileAt(0, 0);
                Debug.Log($"Tile: {piece.currentTile}");
                players.Add(player);
            }
        }

        public void StartGame()
        {
            StartCurrentPlayerTurn();
        }

        public void EndTurn()
        {
            players[currentPlayerIndex].EndTurn();

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            UIManager.Instance.ShowPassDevice(currentPlayerIndex);
        }

        public void Roll()
        {
            players[currentPlayerIndex].RollDiceAndMove();
        }

        public void StartCurrentPlayerTurn()
        {
            players[currentPlayerIndex].StartTurn();
        }
    }
}