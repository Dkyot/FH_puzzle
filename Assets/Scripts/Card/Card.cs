using UnityEngine;
using TMPro;
using Assets.Scripts.Sound;

namespace FH.Cards {
    public class Card : MonoBehaviour {
        public delegate bool OnFlipDelegate(Card card);
        public static event OnFlipDelegate OnFlip;

        public delegate void OnResetDelegate();
        public static event OnResetDelegate OnReset;

        public int Value { get; private set; }
        public CardState CurrentState { get; private set; }

        public SpriteRenderer sprite;
        public SpriteRenderer backSprite;
        public bool isMatched;

        [SerializeField] private AudioClip _flipSound;
        private const float _cardFlipSoundScale = 0.5f;
        private const float _cardTipFlipSoundScale = 0.1f;

        private TextMeshProUGUI cardTextValue;
        private bool isPicked;

        private float rotationSpeed = 0.045f;

        private float timeUntilClosing = 0.55f;
        private float timer;
        private bool delay;

        private float fullRotationSpeed = 0.2f;
        private bool fullRotation = false;
        private bool halfRotationCompleted = false;
        private float rotationTimer = 0;
        private float rotationCooldown = 0.6f;

        private bool _playedSoundOnClose = false;

        float reference;

        private void Awake() {
            sprite = sprite.GetComponent<SpriteRenderer>();
            cardTextValue = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start() {
            CurrentState = CardState.Closed;
        }

        private void Update() {
            if (fullRotation) {
                TipFullRotation();
                return;
            }

            if (isPicked && (CurrentState == CardState.Closed || CurrentState == CardState.Opening))
                OpenRotation();
            else if (isPicked && (CurrentState == CardState.Opened || CurrentState == CardState.Closing))
                CloseRotation();

            if (delay) {
                timer += Time.deltaTime;
                if (timer >= timeUntilClosing) {
                    CloseRotation();
                }
            }
        }

        public void Pick() {
            if (delay)
                return;

            if (OnFlip(this)) {
                SoundManager.Instance.PlayOneShot(_flipSound);
                isPicked = true;
            }
        }

        public void SetValue(int value) {
            this.Value = value;
            if (cardTextValue != null)
                cardTextValue.text = this.Value.ToString();
        }

        public void StartMismatchTimer() {
            delay = true;
        }

        public void StartFullRotation() {
            SoundManager.Instance.PlayOneShot(_flipSound, _cardTipFlipSoundScale);
            fullRotation = true;
        }

        #region Rotation methods
        private void OpenRotation() {
            CurrentState = CardState.Opening;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
                isPicked = false;
                CurrentState = CardState.Opened;
            }
        }

        private void CloseRotation() {
            CurrentState = CardState.Closing;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
                isPicked = false;
                CurrentState = CardState.Closed;

                if (delay) {
                    delay = false;
                    timer = 0;
                    OnReset();
                }
            }
        }

        public void TipFullRotation() {
            if (halfRotationCompleted == false) {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, fullRotationSpeed);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
                    halfRotationCompleted = true;
                }
            }
            else {
                rotationTimer += Time.deltaTime;
                if (rotationTimer < rotationCooldown) return;

                if (!_playedSoundOnClose) {
                    SoundManager.Instance.PlayOneShot(_flipSound, _cardTipFlipSoundScale);
                    _playedSoundOnClose = true;
                }
    
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, fullRotationSpeed);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
                    halfRotationCompleted = false;
                    _playedSoundOnClose = false;
                    fullRotation = false;

                    rotationTimer = 0;

                    CurrentState = CardState.Closed;
                }
            }
        }
        #endregion
    }
}
