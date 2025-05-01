using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demo
{
    public class Player : MonoBehaviour
    {
        public Piece controlledPiece;
        public TextMeshProUGUI ui;
        public int gems { private set; get; }

        private void Update() {
            // not ideal for each tick

            if(ui != null) {
                ui.text = $"{name}: {gems}";
            }
        }

        public void StartTurn()
        {
            Debug.Log($"{name}'s turn started.");
            EventLog.Instance.Log($"It's {name}'s turn");
            // Enable UI for player: Roll, Use Card, Attack, End Turn
        }

        public void EndTurn()
        {
            Debug.Log($"{name}'s turn ended.");
            // Disable player input/UI
        }

        public void RollDiceAndMove()
        {
            int roll = Random.Range(1, 7); // 1-6
            Debug.Log($"{name} rolled a {roll}");
            EventLog.Instance.Log($"{name} rolled a {roll}");
            controlledPiece.MoveSteps(roll);
        }

        public void UseCard()
        {
            Debug.Log($"{name} used a card (stub).");
            EventLog.Instance.Log($"{name} used a card (stub).");
            // Implement later
        }

        public void ChangeGems(int change)
        {
            gems += change;
            gems = math.clamp(gems, 0, 10 );
        }
    }
}