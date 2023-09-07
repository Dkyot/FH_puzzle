using FH.Cards;
using System;
using UnityEngine;

namespace FH.Level {
    public sealed class ScoreCounter : MonoBehaviour {
        public event Action<int> MismatchChanged;
        public event Action<float> TimeChanged;

        public float Time {
            get => timer;
            set {
                timer = value;
                TimeChanged?.Invoke(value);
            }
        }

        public int Mismatch {
            get => mismatchCounter;
            set {
                mismatchCounter = value;
                MismatchChanged?.Invoke(value);
            }
        }

        public float FinalScore => finalScore;

        [SerializeField] private int mismatchCounter;
        [SerializeField] private float timer;
        [SerializeField] private float finalScore;

        private void Awake() {
            AddCardManagerEvents();
            Reset();
        }

        private void IncreaseMismatch() {
            Mismatch++;
        }

        public void Reset() {
            Mismatch = 0;
            Time = 0;
        }

        public void CalculateScore() {
            finalScore = 10000 - (timer * 4) - (mismatchCounter * 17);
            if (finalScore <= 0)
                finalScore = 1;
        }

        public string GetScoreJson() {
            return JsonUtility.ToJson(this);
        }

        #region Subscribe to events
        private void AddCardManagerEvents() {
            CardManager.OnMismatch += IncreaseMismatch;
        }
        #endregion
    }
}
