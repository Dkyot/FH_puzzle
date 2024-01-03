using FH.Inputs;
using FH.SO;
using FH.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using YandexSDK.Scripts;

namespace FH.Cards {
    [RequireComponent(typeof(CardFlipper))]
    public class CardManager : MonoBehaviour {
        private const float offset = 1;

        public event EventHandler OnWin;

        public delegate void OnMatchCheckDelegate();
        public static event OnMatchCheckDelegate OnMatchCheck;

        public delegate void OnResetDelegate();
        public static event OnResetDelegate OnReset;

        public delegate void OnMismatchDelegate();
        public static event OnMismatchDelegate OnMismatch;

        public CardFlipper CardFlipper { get; private set; }

        public int Rows { get; set; }
        public int Colums { get; set; }
        public ColorsSO Pallete { get; set; }
        public bool UseTwoPairs { get; set; }

        [SerializeField] private GameObject _markerPrefab;

        [Header("Scene References")]
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Transform spawnPosition;

        [Header("Card Desc Size")]
        [SerializeField] private float _width;
        [SerializeField] private float _height;

        [Header("Cards Sound")]
        [SerializeField] private AudioClip _cardFlipSound;

        [Header("Particles")]
        [SerializeField] ParticleSystem _destroyParticles;

        private List<Card> cards;
        private int pairCount;

        private Card card1 = null;
        private Card card2 = null;

        private ParticleSystem _firstCardParticleSystem;
        private ParticleSystem _secondCardParticleSystem;

        private GameObject _firstMarker;
        private GameObject _secondMarker;
        private (Card, Card)? _cardHighlightedPair;

        private bool _isRunning = false;
        private float _tipFlipCooldown = 0.05f;
        private float _tipFlipTimer = 0;
        private int _tipFlipIndex = 0;
        private bool _tipFlipEnded = false;
        private List<Card> _waveCardList = new List<Card>();

        private CancellationTokenSource _waveCancellationSource;

        public void FindPair() {
            _firstMarker.SetActive(false);
            _secondMarker.SetActive(false);
            _cardHighlightedPair = null;

            var cardPair = GetTwoEqualCards();
            if (cardPair == null)
                return;

            _cardHighlightedPair = cardPair;
            _firstMarker.SetActive(true);
            _secondMarker.SetActive(true);
            _firstMarker.transform.position = cardPair.Value.firstCard.transform.position;
            _secondMarker.transform.position = cardPair.Value.secondCard.transform.position;
        }

        public async Awaitable WaveTip() {
            if (_isRunning)
                return;

            _waveCancellationSource?.Cancel();
            _waveCancellationSource = new CancellationTokenSource();

            var cancellToken = _waveCancellationSource.Token;

            Reset();

            _isRunning = true;

            _tipFlipTimer = 0;
            _tipFlipIndex = 0;
            _tipFlipEnded = false;

            while (!_tipFlipEnded) {
                if (cancellToken.IsCancellationRequested) {
                    _isRunning = false;
                    return;
                }

                _tipFlipTimer += Time.deltaTime;

                if (_tipFlipTimer >= _tipFlipCooldown) {
                    var card = _waveCardList[_tipFlipIndex];
                    if (card.isActiveAndEnabled) {
                        card.StartFullRotation();
                    }

                    _tipFlipIndex++;
                    _tipFlipTimer = 0;
                }

                if (_tipFlipIndex == _waveCardList.Count) {
                    _tipFlipEnded = true;
                }

                await Awaitable.NextFrameAsync();
            }

            // Wait for all cards closed
            await Awaitable.WaitForSecondsAsync(1f);

            _isRunning = false;
        }

        public void CreateCards() {
            _waveCancellationSource?.Cancel();

            foreach (Card card in cards) {
                Destroy(card.gameObject);
            }

            cards.Clear();
            SpawnCards();
            DistributionOfValues();

            pairCount = cards.Count / 2;

            _waveCardList = TipMegaWaveFlip(cards, Rows, Colums);

            OnReset?.Invoke();
        }

        private void Awake() {
            if (_destroyParticles != null) {
                _firstCardParticleSystem = Instantiate<ParticleSystem>(_destroyParticles);
                _secondCardParticleSystem = Instantiate<ParticleSystem>(_destroyParticles);
            }

            _firstMarker = Instantiate(_markerPrefab);
            _firstMarker.gameObject.SetActive(false);

            _secondMarker = Instantiate(_markerPrefab);
            _secondMarker.gameObject.SetActive(false);

            CardFlipper = GetComponent<CardFlipper>();

            cards = new List<Card>();
            AddCardEvents();
        }

        private void OnDisable() {
            UnsubscribeCardEvents();
        }

        #region Main methods

        private void SpawnCards() {
            Vector2 cardPrefabSize = cardPrefab.backSprite.bounds.size;
            Vector2 cardRequiredSize = new Vector2(_width / Colums, _height / Rows);
            Vector3 cardScale = cardRequiredSize / cardPrefabSize;
            cardScale.z = 1;

            _firstMarker.transform.localScale = cardScale;
            _secondMarker.transform.localScale = cardScale;

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
            if (card.CurrentState == CardState.Closed) {
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
            else if (card.CurrentState == CardState.Opened) {
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
        private (Card firstCard, Card secondCard)? FindTwoEqualCards() {
            List<Card> closedCards = cards.Where(x => x.isMatched == false).ToList();
            if (closedCards.Count == 0)
                return null;

            int firstCardIndex = UnityEngine.Random.Range(0, closedCards.Count);
            var firstCard = closedCards[firstCardIndex];
            int firstCardValue = firstCard.Value;

            foreach (Card card in closedCards) {
                if (card == firstCard || card.Value != firstCardValue)
                    continue;

                return (firstCard, card);
            }

            Debug.Log("WTF??? Could not fiund another card with the same value!!!");
            return null;
        }

        private Card FindEqualCard(Card card) {
            var closedCards = cards.Where(x => x.isMatched == false);

            foreach (Card anotherCard in closedCards) {
                if (anotherCard == card || anotherCard.Value != card.Value)
                    continue;

                return anotherCard;
            }

            Debug.Log("WTF??? Could not fiund another card with the same value!!!");
            return null;
        }

        private (Card firstCard, Card secondCard)? GetTwoEqualCards() {
            (Card firstCard, Card secondCard) cardPair;

            if (card1 == null) {
                var twoEqualCards = FindTwoEqualCards();
                if (twoEqualCards == null)
                    return null;

                cardPair = twoEqualCards.Value;
            }
            else {
                var secondCard = FindEqualCard(card1);
                if (secondCard == null)
                    return null;

                cardPair.firstCard = card1;
                cardPair.secondCard = secondCard;
            }

            return cardPair;
        }

        private List<Card> TipMegaWaveFlip(List<Card> cardsList, int w, int h) {
            List<Card> list = new List<Card>();

            for (int i = 0; i < h; i++) {
                for (int j = 0; j < (i + 1) - (i / w); j++) {
                    // Debug.Log($"{j} {i} {w * i - (w - 1) * j}");
                    list.Add(cardsList[w * i - (w - 1) * j]);
                }
            }

            int start = (h - 1) * w + 1;
            int iter = cardsList.Count - start;

            for (int i = 0; i < iter; i++) {
                int x = 0;
                for (int j = iter - i; j > 0; j--) {
                    list.Add(cardsList[start - x * (w - 1) + i]);
                    x++;
                }
                x = 0;
            }

            return list;
        }
        #endregion

        #region Match Checking
        private void MatchChecking(Card card1, Card card2) {
            if (card1 == null || card2 == null)
                return;

            OnMatchCheck?.Invoke();

            if (card1.Value == card2.Value) {
                //Debug.Log("match!");
                Card obj1 = card1;
                Card obj2 = card2;

                obj1.isMatched = true;
                obj2.isMatched = true;

                CheckHighlightedCards(card1, card2);

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

        private void CheckHighlightedCards(Card card1, Card card2) {
            if (_cardHighlightedPair != null) {
                var hightlightedCards = _cardHighlightedPair.Value;
                if (hightlightedCards.Item1 == card1
                    || hightlightedCards.Item2 == card2
                    || hightlightedCards.Item2 == card1
                    || hightlightedCards.Item2 == card2) {
                    _cardHighlightedPair = null;
                    _firstMarker.SetActive(false);
                    _secondMarker.SetActive(false);
                }
            }
        }

        private void Reset() {
            card1 = null;
            card2 = null;
        }

        private void WinVerification() {
            pairCount--;
            if (pairCount == 0)
                OnWin?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator SetInactive(Card card1, Card card2) {
            yield return new WaitForSeconds(0.25f);

            if (_destroyParticles != null) {
                _firstCardParticleSystem.transform.position = card1.transform.position;
                _secondCardParticleSystem.transform.position = card2.transform.position;

                _firstCardParticleSystem.Play();
                _secondCardParticleSystem.Play();
            }

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

            //if (cards.Count > 36)
            //    return;

            List<Pair<int, Color>> values = new List<Pair<int, Color>>();

            if (UseTwoPairs) {
                FourEqualCards(values);
            }
            else {
                TwoEqualCards(values);
            }

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