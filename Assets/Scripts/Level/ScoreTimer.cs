using UnityEngine;

namespace FH.Level {
    public sealed class ScoreTimer : MonoBehaviour {
        public bool IsRunning {
            get => _isRunning;
            set => _isRunning = value;
        }

        private ScoreCounter _score;
        private bool _isRunning;

        private void Awake() {
            _score = GetComponent<ScoreCounter>();
        }

        public void Update() {
            if (!_isRunning)
                return;

            _score.Time += Time.deltaTime;
        }
    }
}
