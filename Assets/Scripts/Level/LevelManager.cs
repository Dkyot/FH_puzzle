using FH.Cards;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace FH.Level {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private UnityEvent GamePaused;
        [SerializeField] private UnityEvent GameResumed;

        [SerializeField] private UnityEvent GameFinished;

        [SerializeField] private CardManager cardManager;

        [SerializeField] private ScoreTimer scoreTimer;
        [SerializeField] private ScoreCounter scoreCounter;

        private void Awake() {
            cardManager.OnWin += OnWin;
        }

        private void Start() {
            Restart();
        }

        public void Restart() {
            cardManager?.CreateCards();
            scoreCounter?.Reset();
            scoreTimer.IsRunning = true;
        }

        public void PauseGame() {
            scoreTimer.IsRunning = false;
            GamePaused?.Invoke();
        }

        public void ResumeGame() {
            scoreTimer.IsRunning = true;
            GameResumed?.Invoke();
        }

        private void OnWin(object sender, EventArgs e) {
            scoreTimer.IsRunning = false;
            scoreCounter.CalculateScore();
            Debug.Log(scoreCounter.GetScoreJson());
            GameFinished?.Invoke();
        }
    }
}
