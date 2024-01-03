using UnityEngine;

namespace FH.Utils {
    public class SnowParticleSystemresizer : MonoBehaviour {
        private Camera _camera;
        private Vector2 _demention;

        private void Awake() {
            _camera = Camera.main;
            _demention = new Vector2(Screen.width, Screen.height);
        }

        void Start() {
            ResizeSystemShape();

        }

        void Update() {
            if (_demention.x != Screen.width || _demention.y != Screen.height) {
                ResizeSystemShape();
            }
        }

        private void ResizeSystemShape() {
            var p1 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
            var p2 = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane));
            transform.localScale = new Vector3(p1.x - p2.x + 1, 1, 1);
        }
    }
}
