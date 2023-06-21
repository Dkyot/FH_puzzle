using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform spawnPosition;
    
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float offset;

    private List<Card> cards;

    [SerializeField] private Card card1 = null;
    [SerializeField] private Card card2 = null;
    
    private void Awake() {
        cards = new List<Card>();
    }
    
    private void Start() {
        SpawnCards();
    }

    private void SpawnCards() {
        for (int col = 0; col < columns; col++) {
            for (int row = 0; row < rows; row++) {
                Vector3 position = new Vector3((spawnPosition.position.x + (offset * row)), (spawnPosition.position.y + (offset * col)), 0);
                Card card = Instantiate(cardPrefab, position, Quaternion.identity);
                card.name = "card_" + 'c' + col + 'r' + row;
                cards.Add(card);
            }
        }
    }

    public bool TryToFlip(Card card) {
        if (card.currentState == CardState.Closed) {
            if (card1 == null && !card.Equals(card2)) {
                card1 = card;
                return true;
            }
            else if (card2 == null && !card.Equals(card1)) {
                card2 = card;
                return true;
            }
            else return false;
        }
        else if (card.currentState == CardState.Opened) {
            if (card.Equals(card1)) {
                card1 = null;
                return true;
            }
            else if (card.Equals(card2)) {
                card2 = null;
                return true;
            }
            else return false;
        }

        return false;
    }
}
