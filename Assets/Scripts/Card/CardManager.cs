using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform spawnPosition;
    
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float offset;

    private List<Card> cards;
    public int pairCount;

    [SerializeField] private Card card1 = null;
    [SerializeField] private Card card2 = null;

    [SerializeField] private UnityEvent OnWin;
    
    private void Awake() {
        cards = new List<Card>();
    }
    
    private void Start() {
        SpawnCards();
        DistributionOfValues();

        pairCount = cards.Count / 2;
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
                MatchChecking(card1, card2);
                return true;
            }
            else if (card2 == null && !card.Equals(card1)) {
                card2 = card;
                MatchChecking(card1, card2);
                return true;
            }
            return false;
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
            return false;
        }

        return false;
    }

    private void DistributionOfValues() {
        if (cards.Count % 2 != 0) {
            Debug.Log("Необходимо четное количество карт");
            return;
        }

        List<int> shuffled = new List<int>();
        for (int i = 0; i < cards.Count - 1; i += 2) {
            shuffled.Add(i);
            shuffled.Add(i);
        }

        shuffled = Shuffle(shuffled);

        for (int i = 0; i < cards.Count; i++) {
            cards[i].SetValue(shuffled[i]);
        }
    }

    public void Reset() {
        card1 = null;
        card2 = null;
    }

    #region Auxiliary methods
    private void MatchChecking(Card card1, Card card2) {
        if (card1 == null || card2 == null) return;

        if (card1.value == card2.value) {
            Debug.Log("match!");
            Card obj1 = card1;
            Card obj2 = card2;

            obj1.isMatched = true;
            obj2.isMatched = true;

            Reset();
            StartCoroutine(SetInactive(obj1, obj2));

            WinVerification();
        }
        else {
            Debug.Log("mismatch!");
            card1.StartMismatchTimer();
            card2.StartMismatchTimer();
        }
    }

    private void WinVerification() {
        pairCount--;
        if (pairCount == 0)
            OnWin?.Invoke();
    }

    private IEnumerator SetInactive(Card card1, Card card2) {
        yield return new WaitForSeconds(0.25f);
        card1.gameObject.SetActive(false);
        card2.gameObject.SetActive(false);
    }

    private List<int> Shuffle (List<int> list) {
        List<int> randomList = list.OrderBy(x => Random.Range(0, int.MaxValue)).ToList();
        return randomList;
    }
    #endregion
}
