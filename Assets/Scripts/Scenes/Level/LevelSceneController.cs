using FH.Cards;
using FH.Scenes;
using FH.SO;
using System;
using FH.UI.Views.LevelStart;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using FH.Utils;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using FH.UI.Views.LevelCompleted;
using TMPro;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace FH.Level {
    [RequireComponent(typeof(ScoreCounter))]
    [RequireComponent(typeof(ScoreTimer))]
    public class LevelSceneController : MonoBehaviour, ISceneController {
        [Header("Level Events")]
        [SerializeField] private UnityEvent GamePaused;
        [SerializeField] private UnityEvent GameResumed;
        [SerializeField] private UnityEvent GameFinished;

        [Header("System Object References")]
        [SerializeField] private GameContext _gameContext;

        [Header("Level References")]
        [SerializeField] private CardManager cardManager;
        [SerializeField] private SpriteRenderer _levelImage;
        [SerializeField] private LevelStartViewController _starAnimationViewController;
        [SerializeField] private LevelCompletedController _levelCompletedViewController;

        private ScoreTimer scoreTimer;
        private ScoreCounter scoreCounter;

        private AssetReferenceSprite _spriteRef;
        private Sprite _image;

        public async Awaitable StartPreloading() {
            await LoadImage();
            _levelCompletedViewController.SetImage(_image);
            _levelImage.sprite = _image;
        }

        public void StartScene() {
            _ = StartSceneAsync();
        }

        public async Awaitable UnloadScene() {
            _image = null;
            _spriteRef.ReleaseAsset();

            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        public void Restart() {
            cardManager.CreateCards();
            scoreCounter.Reset();
            scoreTimer.IsRunning = true;
        }

        /// <summary> Stops time and card flipper, but doesn't trigger pause event</summary>
        public void FreezeGame() {
            cardManager.CardFlipper.IsEnable = false;
            scoreTimer.IsRunning = false;
        }

        public void UnFreezeGame() {
            cardManager.CardFlipper.IsEnable = true;
            scoreTimer.IsRunning = true;
        }

        public void PauseGame() {
            FreezeGame();
            GamePaused.Invoke();
        }

        public void ResumeGame() {
            UnFreezeGame();
            GameResumed.Invoke();
        }

        public void GoToMainMenu() {
            _gameContext.SceneManagerProxy.RequestMainMenuTrastion();
        }

        public void NextLevel() {
            var currentLevel = _gameContext.CurrentLevel;
            LevelDataSO nextLevel = null;
            bool findCurrent = false;

            foreach (var level in _gameContext.LevelDataBase.Levels) {
                if (!findCurrent) {
                    if (level == currentLevel)
                        findCurrent = true;
                    continue;
                }

                nextLevel = level;
                break;
            }

            if (nextLevel == null)
                return;

            _gameContext.CurrentLevel = nextLevel;
            _gameContext.SceneManagerProxy.RequestLevelTransition();
        }

        private void Awake() {
            scoreCounter = GetComponent<ScoreCounter>();
            scoreTimer = GetComponent<ScoreTimer>();
        }

        private void Start() {
            FreezeGame();

            var levelNumberVariable = _gameContext.GlobalGroupVariables["levelNumber"];
            if (levelNumberVariable is StringVariable stringVariable) {
                stringVariable.Value = _gameContext.CurrentLevel.number.ToString();
            }

            var levelData = _gameContext.CurrentLevel;
            cardManager.Colums = levelData.Params.Columns;
            cardManager.Pallete = levelData.Params.Palete;
            cardManager.Rows = levelData.Params.Rows;

            cardManager.OnWin += OnWin;

            cardManager.CreateCards();

            _gameContext.SceneManagerProxy.SceneController = this;
        }

        private async Awaitable StartSceneAsync() {
            await _starAnimationViewController.StartAnimation();

            scoreCounter.Reset();
            UnFreezeGame();
        }

        private void OnWin(object sender, EventArgs e) {
            FreezeGame();
            scoreCounter.CalculateScore();

            var currentLevel = _gameContext.CurrentLevel;
            currentLevel.isCompleted = true;
            currentLevel.score = scoreCounter.FinalScore;

            GameFinished.Invoke();
        }

        private async Awaitable LoadImage() {
            _spriteRef = _gameContext.CurrentLevel.LevelImage;
            var operation = await _spriteRef.LoadAssetAsync().CompleteAsync();

            if (operation.Status == AsyncOperationStatus.Failed) {
                Debug.LogException(operation.OperationException);
                return;
            }

            var result = operation.Result;
            if (result == null) {
                Debug.LogError("Failed load image");
                return;
            }

            _image = result;
        }
    }
}
