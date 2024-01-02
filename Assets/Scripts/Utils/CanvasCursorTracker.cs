using FH.Inputs;
using UnityEngine;

namespace FH.Utils {
    [RequireComponent(typeof(RectTransform))]
    public sealed class CanvasCursorTracker : MonoBehaviour {
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private Canvas parent;

        private RectTransform rect;

        private void Awake() {
            rect = GetComponent<RectTransform>();
        }

        void Update() {
            var position = inputHandler.MousePosition / parent.scaleFactor;
            rect.anchoredPosition = position;
        }
    }
}
