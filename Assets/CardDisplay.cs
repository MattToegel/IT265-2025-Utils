using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI valueText;

    private Deck.Card card;

    public void SetCard(Deck.Card newCard) {
        card = newCard;
        UpdateVisual();
    }

    public Deck.Card GetCard() {
        return card;
    }

    private void UpdateVisual() {
        if (valueText != null && card != null) {
            valueText.text = card.isFaceUp ? $"{card.value}{card.suit}" : ""; // face down
        }
    }

    // Optional: toggle visibility
    public void FlipCard(bool faceUp) {
        card.isFaceUp = faceUp;
        UpdateVisual();
    }
}
