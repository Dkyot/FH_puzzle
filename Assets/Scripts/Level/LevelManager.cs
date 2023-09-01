using System;
using UnityEngine;
using UnityEngine.UI;

namespace FH.Level {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private CardManager cardManager;

        [SerializeField] private ScoreTimer scoreTimer;
        [SerializeField] private ScoreCounter scoreCounter;

        private void Awake() {
            cardManager = cardManager.GetComponent<CardManager>();
            cardManager.OnWin += OnWin;

            scoreCounter = scoreCounter.GetComponent<ScoreCounter>();
        }

        private void Start() {
            Restart();
        }

        public void Restart() {
            cardManager?.CreateCards();
            scoreCounter?.Reset();
            scoreTimer.IsRunning = true;
        }

        private void OnWin(object sender, EventArgs e) {
            scoreTimer.IsRunning = false;
            scoreCounter.CalculateScore();
            Debug.Log(scoreCounter.GetScoreJson());
        }
    }
}
