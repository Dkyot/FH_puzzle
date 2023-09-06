using FH.Cards;
using FH.Scenes;
using FH.SO;
using System;
using FH.UI.Views.LevelStart;
using UnityEngine;
using UnityEngine.Events;

namespace FH.Level {
    [RequireComponent(typeof(ScoreCounter))]
    [RequireComponent(typeof(ScoreTimer))]
    public class LevelManager : MonoBehaviour, ISceneController {
        [Header("Level Events")]
        [SerializeField] private UnityEvent GamePaused;
        [SerializeField] private UnityEvent GameResumed;
        [SerializeField] private UnityEvent GameFinished;

        [Header("System Object References")]
        [SerializeField] private LevelContext _levelContext;
        [SerializeField] private SceneManagerProxy _sceneManagerProxy;

        [Header("Level References")]
        [SerializeField] private CardManager cardManager;
        [SerializeField] private LevelStartViewController _starAnimationView;

        private ScoreTimer scoreTimer;
        private ScoreCounter scoreCounter;

        private void Awake() {
            scoreCounter = GetComponent<ScoreCounter>();
            scoreTimer = GetComponent<ScoreTimer>();
        }

        private void Start() {
            cardManager.CardFlipper.IsEnable = false;

            var levelData = _levelContext.currentLevel;
            cardManager.columns = levelData.Params.Columns;
            cardManager.pallete = levelData.Params.Palete;
            cardManager.rows = levelData.Params.Rows;

            cardManager.OnWin += OnWin;

            cardManager.CreateCards();

            _sceneManagerProxy.SceneController = this;
        }

        public async Awaitable StartPreloading() {
            return;
        }

        public void StartScene() {
            _ = StartSceneAsync();
        }

        public void Restart() {
            cardManager.CreateCards();
            scoreCounter.Reset();
            scoreTimer.IsRunning = true;
        }

        public void PauseGame() {
            cardManager.CardFlipper.IsEnable = false;
            scoreTimer.IsRunning = false;
            GamePaused.Invoke();
        }

        public void ResumeGame() {
            cardManager.CardFlipper.IsEnable = true;
            scoreTimer.IsRunning = true;
            GameResumed.Invoke();
        }

        private async Awaitable StartSceneAsync() {
            await _starAnimationView.StartAnimation();

            cardManager.CardFlipper.IsEnable = true;

            scoreCounter.Reset();
            scoreTimer.IsRunning = true;
        }

        private void OnWin(object sender, EventArgs e) {
            scoreTimer.IsRunning = false;
            scoreCounter.CalculateScore();

            Debug.Log(scoreCounter.GetScoreJson());
            GameFinished.Invoke();
        }

    }
}
