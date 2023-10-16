using UnityEngine;

namespace FH.Level {
    [RequireComponent(typeof(ScoreCounter))]
    public sealed class ScoreTimer : MonoBehaviour {
        private ScoreCounter _score;
        private int _lockCounter = 0;

        public void Lock() {
            _lockCounter++;
        }

        public void Unlock() {
            _lockCounter = Mathf.Max(_lockCounter - 1, 0);
        }

        private void Awake() {
            _score = GetComponent<ScoreCounter>();
        }

        public void Update() {
            if (_lockCounter != 0) return;
            _score.Time += Time.deltaTime;
        }
    }
}
