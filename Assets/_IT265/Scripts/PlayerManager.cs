using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        [SerializeField] private CameraFollower follow;
        [SerializeField] private Transform container;

        public List<Player> players = new();
        private int currentPlayerIndex = 0;

        public GameObject playerPrefab;
        public Transform piecesParent; // where spawned pieces go
        public Piece startingPiecePrefab; // piece to assign each player
        
        private void Awake()
        {
            Instance = this;
        }

        public Player CurrentPlayer()
        {
            return players[currentPlayerIndex];
        }
        // TODO add logic for piece getting defeated (reset to start?)

        // TODO add logic for collecting something (game currency/win condition)
        public void CreatePlayers(int count)
        {
            players.Clear();

            for (int i = 0; i < count; i++)
            {
                GameObject playerObj = Instantiate(playerPrefab);
                playerObj.transform.parent = container ;
                playerObj.name = $"Player {i + 1}";
                Player player = playerObj.GetComponent<Player>();

                Piece piece = Instantiate(startingPiecePrefab, piecesParent);
                player.controlledPiece = piece;
                // TODO: Assign starting tile like 0,0 later here
                piece.SetTile(Tile.GetTileAt(0, 0));
                piece.controller = player;
                Debug.Log($"Tile: {piece.currentTile}");
                piece.Init();
                players.Add(player);
            }
        }

        public void StartGame()
        {
            StartCurrentPlayerTurn();
        }

        public void EndTurn()
        {
            CurrentPlayer().EndTurn();

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            UIManager.Instance.ShowPassDevice(currentPlayerIndex);
        }

        public void Roll()
        {
            CurrentPlayer().RollDiceAndMove();
            UIManager.Instance.rollDiceButton.gameObject.SetActive(false);
        }

        public void StartCurrentPlayerTurn()
        {
            
            follow.FocusOnTarget(CurrentPlayer().controlledPiece.transform);
            UIManager.Instance.rollDiceButton.gameObject.SetActive(true);
            CurrentPlayer().StartTurn();
        }
    }
}