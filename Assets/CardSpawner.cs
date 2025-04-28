using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSpawner : MonoBehaviour {
    public Deck deck;
    public GameObject cardPrefab;
    public Transform container;
    public int numberOfCardsToDraw = 5;
    public float spacing = 1.5f;

    private void Start() {
        if (deck == null || cardPrefab == null || container == null) {
            Debug.LogError("Missing references in CardSpawner.");
            return;
        }

        SpawnCards();
    }

    public void SpawnCards() {
        List<Deck.Card> drawnCards = deck.DrawCards(numberOfCardsToDraw);

        for (int i = 0; i < drawnCards.Count; i++) {
            Deck.Card card = drawnCards[i];

            // Instantiate prefab
            GameObject cardObject = Instantiate(cardPrefab, container);

            // Position the card in a line
            cardObject.transform.localPosition = new Vector3(i * spacing, 0, 0);

            // Set the TMP text
            TMP_Text tmp = cardObject.GetComponentInChildren<TMP_Text>();
            if (tmp != null) {
                tmp.text = $"{card.value}{card.suit}";
            }
            else {
                Debug.LogWarning($"Card prefab missing TMP_Text in child for card {card.value}{card.suit}");
            }

            // Optional: Name the object
            cardObject.name = $"Card_{card.value}{card.suit}";
        }
    }
}
