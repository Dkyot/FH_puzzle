using FH.Inputs;
using FH.SO;
using FH.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FH.Cards {
    [RequireComponent(typeof(CardFlipper))]
    public class CardManager : MonoBehaviour {
        private const float offset = 1;

        public CardFlipper CardFlipper { get; private set; }

        public int Rows { get; set; }
        public int Colums { get; set; }
        public ColorsSO Pallete { get; set; }

        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform spawnPosition;

        [SerializeField] private float _width;
        [SerializeField] private float _height;

        private List<Card> cards;
        private int pairCount;

        private Card card1 = null;
        private Card card2 = null;

        public event EventHandler OnWin;

        public delegate void OnMatchCheckDelegate();
        public static event OnMatchCheckDelegate OnMatchCheck;

        public delegate void OnResetDelegate();
        public static event OnResetDelegate OnReset;

        public delegate void OnMismatchDelegate();
        public static event OnMismatchDelegate OnMismatch;

        private void Awake() {
            CardFlipper = GetComponent<CardFlipper>();

            cards = new List<Card>();
            AddCardEvents();
            AddTipsEvents();
        }

        private void OnDisable() {
            UnsubscribeCardEvents();
            UnsubscribeTipsEvents();
        }

        public void CreateCards() {
            foreach (Card card in cards) {
                Destroy(card.gameObject);
            }

            cards.Clear();
            SpawnCards();
            DistributionOfValues();

            pairCount = cards.Count / 2;

            OnReset?.Invoke();
        }

        #region Main methods
        private void SpawnCards() {
            Vector2 cardPrefabSize = cardPrefab.backSprite.bounds.size;
            Vector2 cardRequiredSize = new Vector2(_width / Colums, _height / Rows);
            Vector3 cardScale = cardRequiredSize / cardPrefabSize;
            cardScale.z = 1;

            float cardHalfWidth = cardRequiredSize.x / 2;
            float cardHalfHeight = cardRequiredSize.y / 2;

            float halfWidth = _width / 2;
            float halfHeight = _height / 2;

            float rowY = spawnPosition.position.y - halfHeight;

            for (int col = 0; col < Colums; col++) {
                float colX = spawnPosition.position.x - halfWidth;

                for (int row = 0; row < Rows; row++) {
                    var cardPosition = new Vector3(
                        colX + cardHalfWidth,
                        rowY + cardHalfHeight,
                        0
                    );

                    Card card = Instantiate(cardPrefab);
                    card.transform.position = cardPosition;
                    card.transform.localScale = cardScale;

                    card.name = "card_" + 'c' + col + 'r' + row;
                    cards.Add(card);

                    colX += cardRequiredSize.x;
                }

                rowY += cardRequiredSize.y;
            }
        }

        private bool TryToFlip(Card card) {
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
        #endregion


        #region Tip methods
        private void FindEqualCards() {
            List<Card> closedCards = cards.Where(x => x.isMatched == false).ToList();
            if (closedCards.Count == 0)
                return;

            int firstCardIndex = UnityEngine.Random.Range(0, closedCards.Count);
            int firstCardValue = closedCards[firstCardIndex].value;

            foreach (Card card in closedCards) {
                if (card.value == firstCardValue)
                    if (!card.Equals(closedCards[firstCardIndex])) {
                        SuggestionOfPositions(card, closedCards[firstCardIndex]);
                    }
            }
        }

        private void SuggestionOfPositions(Card first, Card second) {
            //Debug.Log(first.name);
            //Debug.Log(second.name);
        }
        #endregion

        #region Match Checking
        private void MatchChecking(Card card1, Card card2) {
            if (card1 == null || card2 == null)
                return;

            OnMatchCheck?.Invoke();

            if (card1.value == card2.value) {
                //Debug.Log("match!");
                Card obj1 = card1;
                Card obj2 = card2;

                obj1.isMatched = true;
                obj2.isMatched = true;

                Reset();
                StartCoroutine(SetInactive(obj1, obj2));

                WinVerification();
            }
            else {
                OnMismatch?.Invoke();

                //Debug.Log("mismatch!");
                card1.StartMismatchTimer();
                card2.StartMismatchTimer();
            }
        }

        private void Reset() {
            card1 = null;
            card2 = null;
        }

        private void WinVerification() {
            pairCount--;
            if (pairCount == 0)
                if (OnWin != null)
                    OnWin(this, EventArgs.Empty);
        }

        private IEnumerator SetInactive(Card card1, Card card2) {
            yield return new WaitForSeconds(0.25f);
            card1.gameObject.SetActive(false);
            card2.gameObject.SetActive(false);
        }
        #endregion

        #region Distribution Of Values
        private void DistributionOfValues() {
            if (cards.Count % 2 != 0) {
                //Debug.Log("Необходимо четное количество карт!");
                return;
            }

            if (cards.Count > 36)
                return;

            List<Pair<int, Color>> values = new List<Pair<int, Color>>();

            TwoEqualCards(values);
            //FourEqualCards(values);

            values = Shuffle(values);

            for (int i = 0; i < cards.Count; i++) {
                cards[i].SetValue(values[i].First);
                cards[i].sprite.color = values[i].Second;
            }
        }

        private void TwoEqualCards(List<Pair<int, Color>> values) {
            for (int i = 0; i < cards.Count / 2; i++) {
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
            }
        }

        private void FourEqualCards(List<Pair<int, Color>> values) {
            for (int i = 0; i < cards.Count / 4; i++) {
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
                values.Add(new Pair<int, Color>(i, Pallete.pallete[i]));
            }
        }

        private List<T> Shuffle<T>(List<T> list) {
            List<T> randomList = list.OrderBy(x => UnityEngine.Random.Range(0, int.MaxValue)).ToList();
            return randomList;
        }
        #endregion

        #region Subscribe to events
        private void AddCardEvents() {
            Card.OnFlip += TryToFlip;
            Card.OnReset += Reset;
        }

        private void UnsubscribeCardEvents() {
            Card.OnFlip -= TryToFlip;
            Card.OnReset -= Reset;
        }

        private void AddTipsEvents() {
            CardTipsController.OnActivate += FindEqualCards;
        }

        private void UnsubscribeTipsEvents() {
            CardTipsController.OnActivate -= FindEqualCards;
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (spawnPosition == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnPosition.position, new Vector3(_width, _height, 0));
        }
#endif
    }
}