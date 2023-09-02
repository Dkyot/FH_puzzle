using FH.Cards;
using UnityEngine;

namespace FH.Inputs {
    public class CardFlipper : MonoBehaviour {
        [SerializeField] private PlayerInputHandler inputHandler;

        private void Start() {
            inputHandler.Pressed += PerformeFlip;
        }

        public void PerformeFlip(Vector2 screenPosition) {
            Vector3 mousePosition = screenPosition;
            mousePosition.z = Camera.main.nearClipPlane;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider == null)
                return;

            Interaction(hit);
        }

        private void Interaction(RaycastHit2D hit) {
            if (hit.collider.gameObject.TryGetComponent<Card>(out var card))
                card.Pick();
        }
    }
}
