using UnityEngine;

namespace FH.Utils {
    [RequireComponent(typeof(Canvas))]
    public sealed class UIParticleEmiterResizer : MonoBehaviour {
        [SerializeField] private RectTransform _emiter;

        private RectTransform _canvasRectTransform;
        private bool _started = false;

        private void Start() {
            _started = true;
            _canvasRectTransform = GetComponent<RectTransform>();
            UpdateEminerScale();
        }

        private void OnRectTransformDimensionsChange() {
            // This event will be fired even before Start method.
            // So we need to chech the state of component.
            if (!_started) return;
            UpdateEminerScale();
        }

        private void UpdateEminerScale() {
            _emiter.localScale = new Vector3(_canvasRectTransform.sizeDelta.x + 100, 1, 1);
            //_emiter.position = new Vector3(0, 100, 0);
        }
    }
}