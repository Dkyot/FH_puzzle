using UnityEngine;
using TMPro;

namespace FH.Cards {
    public class Card : MonoBehaviour {
        public int value { get; private set; }
        public SpriteRenderer sprite;
        public SpriteRenderer backSprite;
        public AudioSource swipeSound;
        private TextMeshProUGUI cardTextValue;

        private bool isPicked;
        public bool isMatched;

        private float rotationSpeed = 0.045f;

        public CardState currentState { get; private set; }

        public delegate bool OnFlipDelegate(Card card);
        public static event OnFlipDelegate OnFlip;

        public delegate void OnResetDelegate();
        public static event OnResetDelegate OnReset;

        private float timeUntilClosing = 0.55f;
        private float timer;
        private bool delay;

        private float fullRotationSpeed = 0.2f;
        private bool fullRotation = false;
        private bool halfRotationCompleted = false;
        private float rotationTimer = 0;
        private float rotationCooldown = 0.6f;

        float reference;

        private void Awake() {
            sprite = sprite.GetComponent<SpriteRenderer>();
            cardTextValue = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start() {
            currentState = CardState.Closed;
        }

        private void Update() {
            if (fullRotation) {
                TipFullRotation();
                return;
            }

            if (isPicked && (currentState == CardState.Closed || currentState == CardState.Opening))
                OpenRotation();
            else if (isPicked && (currentState == CardState.Opened || currentState == CardState.Closing))
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
                isPicked = true;
                swipeSound.Play();
            }
        }

        public void SetValue(int value) {
            this.value = value;
            if (cardTextValue != null)
                cardTextValue.text = this.value.ToString();
        }

        public void StartMismatchTimer() {
            delay = true;
        }

        public void StartFullRotation() {
            fullRotation = true;
        }

        #region Rotation methods
        private void OpenRotation() {
            currentState = CardState.Opening;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
                isPicked = false;
                currentState = CardState.Opened;
            }
        }

        private void CloseRotation() {
            currentState = CardState.Closing;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
                isPicked = false;
                currentState = CardState.Closed;

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
    
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, fullRotationSpeed);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
                    halfRotationCompleted = false;
                    fullRotation = false;
                    rotationTimer = 0;

                    currentState = CardState.Closed;
                }
            }
        }
        #endregion
    }
}
