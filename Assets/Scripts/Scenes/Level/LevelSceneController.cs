using FH.Cards;
using FH.Scenes;
using FH.SO;
using System;
using System.Linq;
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
using FH.Sound;
using PlatformsSdk.Main;
using PlatformsSdk.MetrikaFeatures;
using PlatformsSdk.UserFeatures;

namespace FH.Level
{
    [RequireComponent(typeof(ScoreCounter))]
    [RequireComponent(typeof(ScoreTimer))]
    public class LevelSceneController : MonoBehaviour, ISceneController
    {
        private static int _trackCounter;

        [Header("Level Events")] [SerializeField]
        private UnityEvent GamePaused;

        [SerializeField] private UnityEvent GameResumed;
        [SerializeField] private UnityEvent GameFinished;

        [Header("System Object References")] [SerializeField]
        private GameContext _gameContext;

        [Header("Level References")] [SerializeField]
        private CardManager cardManager;

        [SerializeField] private SpriteRenderer _levelImage;
        [SerializeField] private LevelStartViewController _starAnimationViewController;
        [SerializeField] private LevelCompletedController _levelCompletedViewController;
        [SerializeField] private TipsPointerController _tipsPointerController;

        [Header("Music")] [SerializeField] private AudioClip _music1;
        [SerializeField] private AudioClip _music2;

        private ScoreTimer scoreTimer;
        private ScoreCounter scoreCounter;

        private Sprite _image;
        private LevelDataSO _curentLevelData;

        public async Awaitable StartPreloading()
        {
            MusicManager.Instance?.FadeIn(0.5f, GetCurrentTrack(), true);
            await LoadImage();
            _levelCompletedViewController.SetImage(_image);
            _levelImage.sprite = _image;
        }

        public void StartScene()
        {
            _ = StartSceneAsync();
        }

        public async Awaitable UnloadScene()
        {
            _image = null;
            _curentLevelData.ReleaseImage();

            await MusicManager.Instance?.FadeOut(0.3f);
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        public void Restart()
        {
            cardManager.CreateCards();
            scoreCounter.Reset();
            scoreTimer.Unlock();
        }

        /// <summary> Stops time and card flipper, but doesn't trigger pause event</summary>
        public void FreezeGame()
        {
            cardManager.CardFlipper.Lock();
            scoreTimer.Lock();
        }

        public void UnFreezeGame()
        {
            cardManager.CardFlipper?.Unlock();
            scoreTimer.Unlock();
        }

        public void PauseGame()
        {
            FreezeGame();
            GamePaused.Invoke();
        }

        public void ResumeGame()
        {
            UnFreezeGame();
            GameResumed.Invoke();
        }

        public void GoToMainMenu()
        {
            _gameContext.SceneManagerProxy.RequestMainMenuTrastion();
        }

        public void NextLevel()
        {
            var nextLevel = GetNextLevel();

            if (nextLevel == null)
                return;

            _gameContext.CurrentLevel = nextLevel;
            _gameContext.SceneManagerProxy.RequestLevelTransition();
        }

        public void ShowReviewGame()
        {
            PlatformFeatures.User.OpenReviewGame();
        }

        private void Awake()
        {
            scoreCounter = GetComponent<ScoreCounter>();
            scoreTimer = GetComponent<ScoreTimer>();
        }

        private void Start()
        {
            FreezeGame();

            _curentLevelData = _gameContext.CurrentLevel;

            // Hide next level button if there are no next level
            // Show game rate button if there are no next level
            bool hasNextLevel = GetNextLevel() != null;
            if (!hasNextLevel)
            {
                _levelCompletedViewController.HideNextLevelButton();
                _levelCompletedViewController.ShowRateGameButton();
            }
            else
            {
                _levelCompletedViewController.HideRateGameButton();
                _levelCompletedViewController.ShowNextLevelButton();
            }


            // Set current level number to global variable
            var levelNumberVariable = _gameContext.GlobalGroupVariables["levelNumber"];
            if (levelNumberVariable is StringVariable stringVariable)
            {
                stringVariable.Value = _gameContext.CurrentLevel.number.ToString();
            }

            // Configure card manager
            var levelData = _gameContext.CurrentLevel;
            cardManager.UseTwoPairs = levelData.Params.UseTwoPair;
            cardManager.Colums = levelData.Params.Columns;
            cardManager.Pallete = levelData.Params.Palete;
            cardManager.Rows = levelData.Params.Rows;
            cardManager.OnWin += OnWin;

            cardManager.CreateCards();

            // Finish initialization 
            _gameContext.SceneManagerProxy.SceneController = this;
        }

        private async Awaitable StartSceneAsync()
        {
            await _starAnimationViewController.StartAnimation();

            scoreCounter.Reset();
            UnFreezeGame();

            switch (_gameContext.CurrentLevel.number)
            {
                case 1:
                    cardManager.FindPair();
                    break;
                case 2:
                    _tipsPointerController.ShowTipsPointer();
                    break;
            }
        }

        private void OnWin(object sender, EventArgs e)
        {
            FreezeGame();
            scoreCounter.CalculateScore();

            var currentLevel = _gameContext.CurrentLevel;
            currentLevel.isCompleted = true;
            currentLevel.score = scoreCounter.FinalScore;

            if (!PlatformFeatures.Save.SaveInfo.LevelsScore.TryGetValue(currentLevel.number, out float score))
            {
                Debug.Log("New level complete");
                PlatformFeatures.Save.SaveInfo.LevelsScore.Add(currentLevel.number, currentLevel.score);
            }
            else
            {
                if (currentLevel.score > score)
                {
                    Debug.Log($"New record level. Old {score}, new {currentLevel.score}");
                    PlatformFeatures.Save.SaveInfo.LevelsScore[currentLevel.number] = currentLevel.score;
                }
            }

            PlatformFeatures.Save.SaveData();
            PlatformFeatures.User.SetMainLeaderboardScore(
                (int)PlatformFeatures.Save.SaveInfo.LevelsScore.Sum(x => x.Value));
            MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.LevelCompleted.ToString() + currentLevel.number);

            GameFinished.Invoke();
        }

        private async Awaitable LoadImage()
        {
            _image = await _gameContext.CurrentLevel.GetLevelImageAsync();
        }

        private LevelDataSO GetNextLevel()
        {
            var currentLevel = _gameContext.CurrentLevel;
            LevelDataSO nextLevel = null;
            bool findCurrent = false;

            foreach (var level in _gameContext.LevelDataBase.Levels)
            {
                if (!findCurrent)
                {
                    if (level == currentLevel)
                        findCurrent = true;
                    continue;
                }

                nextLevel = level;
                break;
            }

            return nextLevel;
        }

        private AudioClip GetCurrentTrack()
        {
            AudioClip track = null;
            if (_trackCounter % 2 == 0)
            {
                track = _music1;
            }
            else
            {
                track = _music2;
            }

            _trackCounter++;
            return track;
        }
    }
}