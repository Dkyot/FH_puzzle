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
}
