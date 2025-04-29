using UnityEngine;

namespace Demo
{
    public class Player : MonoBehaviour
    {
        public Piece controlledPiece;

        public void StartTurn()
        {
            Debug.Log($"{name}'s turn started.");
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

            controlledPiece.MoveSteps(roll);
        }

        public void UseCard()
        {
            Debug.Log($"{name} used a card (stub).");
            // Implement later
        }

        public void Attack()
        {
            Debug.Log($"{name} attempted an attack (stub).");
            // Implement later
        }
    }
}