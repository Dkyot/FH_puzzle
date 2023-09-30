using FH.Cards;
using UnityEngine;

namespace FH.Inputs {
    public class CardFlipper : MonoBehaviour {
        [SerializeField] private PlayerInputHandler _inputHandler;
        private int _lockCounter = 0;

        public void Lock() {
            _lockCounter++;
        }

        public void Unlock() {
            _lockCounter = Mathf.Max(_lockCounter - 1, 0);
        }

        public void PerformeFlip(Vector2 screenPosition) {
            if (_lockCounter != 0) return;

            Vector3 mousePosition = screenPosition;
            mousePosition.z = Camera.main.nearClipPlane;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider == null)
                return;

            Interaction(hit);
        }

        private void Start() {
            _inputHandler.Pressed += PerformeFlip;
        }

        private void Interaction(RaycastHit2D hit) {
            if (hit.collider.gameObject.TryGetComponent<Card>(out var card))
                card.Pick();
        }
    }
}
