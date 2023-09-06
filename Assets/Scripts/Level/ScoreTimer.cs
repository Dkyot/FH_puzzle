using UnityEngine;

namespace FH.Level {
    [RequireComponent(typeof(ScoreCounter))]
    public sealed class ScoreTimer : MonoBehaviour {
        public bool IsRunning { get; set; }

        private ScoreCounter _score;

        private void Awake() {
            _score = GetComponent<ScoreCounter>();
        }

        public void Update() {
            if (!IsRunning)
                return;

            _score.Time += Time.deltaTime;
        }
    }
}
