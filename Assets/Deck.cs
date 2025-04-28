using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour {
    [System.Serializable]
    public class Card {
        public string value;   // A, 10, K, etc.
        public string suit;    // H, D, C, S
        public int intValue;   // 2-14 or 1-13 based on value of Ace
        public bool isFaceUp;  // Whether the card is currently face up
        public Sprite sprite;  // Optional visual representation

        public override string ToString() {
            return $"{value}{suit} ({(isFaceUp ? "Face Up" : "Face Down")})";
        }
    }

    public List<Card> cards = new List<Card>();

    [SerializeField] private TextMeshProUGUI cardCountText;

    private void Start() {
        InitializeStandardDeck();
        ShuffleDeck();
        RefreshCardCountUI();
    }

    public void InitializeStandardDeck() {
        cards.Clear();

        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] suits = { "H", "D", "C", "S" };

        foreach (var value in values) {
            int intVal = GetIntValue(value);
            foreach (var suit in suits) {
                cards.Add(new Card {
                    value = value,
                    suit = suit,
                    intValue = intVal,
                    isFaceUp = false,
                    sprite = null
                });
            }
        }

        RefreshCardCountUI();
    }

    private int GetIntValue(string value) {
        return value switch {
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => 1,
            _ => int.TryParse(value, out int result) ? result : 0
        };
    }

    public void ShuffleDeck() {
        for (int i = 0; i < cards.Count; i++) {
            int rand = Random.Range(i, cards.Count);
            var temp = cards[i];
            cards[i] = cards[rand];
            cards[rand] = temp;
        }

        RefreshCardCountUI();
    }

    public List<Card> DrawCards(int count = 1) {
        List<Card> drawn = new List<Card>();

        for (int i = 0; i < count && cards.Count > 0; i++) {
            drawn.Add(cards[0]);
            cards.RemoveAt(0);
        }

        RefreshCardCountUI();
        return drawn;
    }

    public Card GetCard(string value, string suit) {
        return cards.Find(c => c.value == value && c.suit == suit);
    }

    public void AddCardToTop(Card card) {
        cards.Insert(0, card);
        RefreshCardCountUI();
    }

    public void AddCardToBottom(Card card) {
        cards.Add(card);
        RefreshCardCountUI();
    }

    public void AddCardRandomly(Card card) {
        int index = Random.Range(0, cards.Count + 1);
        cards.Insert(index, card);
        RefreshCardCountUI();
    }

    public int Size() {
        return cards.Count;
    }

    public void PrintDeck() {
        foreach (var card in cards) {
            Debug.Log(card);
        }
    }

    private void RefreshCardCountUI() {
        if (cardCountText != null) {
            cardCountText.text = $"Cards: {cards.Count}";
        }
    }
}
