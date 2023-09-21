using EasyTransition;
using FH.SO;
using FH.Utils;
using NaughtyAttributes;
using System;
using System.Linq;
using SkibidiRunner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using YandexSDK.Scripts;

namespace Assets.Scripts.InitialScene {
    public sealed class InitialSceneController : MonoBehaviour {
        [Header("Registred Scenes")]
        [SerializeField, Scene] private string _mainMenuScene;
        [SerializeField, Scene] private string _levelScene;

        [Header("System Referenses")]
        [SerializeField] private GameContext _gameContext;

        [Header("Level References")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private TransitionSettings _transitionSettings;
        [SerializeField] private ScrollingBgTextureController _textureController;

        private bool _isLoading = false;

        private void Start() {
            ShowLoadingScreen();

            _gameContext.SceneManagerProxy.IsManaged = true;
            _gameContext.SceneManagerProxy.MainMenuTransitionRequested += () => { if (!_isLoading) LoadMainMenuScene(); };
            _gameContext.SceneManagerProxy.LevelTransitionRequested += () => { if (!_isLoading) LoadLevelScene(); };

            _ = InitGame();
        }

        private async Awaitable InitGame() {
            // Init game here
            await PlayerDataLoader.Instance.TryLoadAwaitable(20);
            
            var data = LocalYandexData.Instance.SaveInfo.LevelsScore;
            for (int i = 0; i < data.Count; i++)
            {
                var element = _gameContext.LevelDataBase.Levels.ElementAt(i);
                element.score = data[i];
                element.isCompleted = true;
            }
            
            int index = 1;
            foreach (var level in _gameContext.LevelDataBase.Levels) { 
                level.number = index++;
            }

            // Load Main Menu
            Debug.Log("API READY");
            YandexGamesManager.ApiReady();
            await LoadMainMenuScene();
        }

        private Awaitable LoadMainMenuScene() {
            return LoadScene(_mainMenuScene, false);
        }

        private Awaitable LoadLevelScene() {
            return LoadScene(_levelScene, true);
        }

        private async Awaitable LoadScene(string sceneName, bool showAd) {
            if (_isLoading)
                return;

            _isLoading = true;

            if (_gameContext.SceneManagerProxy.SceneController != null) {
                await ExitCurrentScene();
                await Awaitable.WaitForSecondsAsync(2f);
            }

            if (showAd) {
                await FullscreenAdManager.Instance.ShowAdAwaitable();
            }

            await LoadNewScene(sceneName);
        }

        private async Awaitable LoadNewScene(string sceneName) {
            _gameContext.SceneManagerProxy.SceneControllerSet += OnControllerSet;

            try {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                var scene = SceneManager.GetSceneByName(sceneName);
                SceneManager.SetActiveScene(scene);
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private async Awaitable ExitCurrentScene() {
            await StartTransition();
            ShowLoadingScreen();

            try {
                await _gameContext.SceneManagerProxy.SceneController.UnloadScene();
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private void OnControllerSet() {
            _gameContext.SceneManagerProxy.SceneControllerSet -= OnControllerSet;
            _ = OnControllerSetAsync();
        }

        private async Awaitable OnControllerSetAsync() {
            await PrepareScene();
            await Awaitable.NextFrameAsync();
            await EnterScene();
            _isLoading = false;
        }

        private async Awaitable PrepareScene() {
            await _gameContext.SceneManagerProxy.SceneController.StartPreloading();
        }

        private async Awaitable EnterScene() {
            await StartTransition();
            HideLoadingScreen();
            _gameContext.SceneManagerProxy.SceneController.StartScene();
        }

        private Awaitable StartTransition() {
            TransitionManager.Instance().Transition(_transitionSettings, 0);
            return Awaitable.WaitForSecondsAsync(1f);
        }


        private void ShowLoadingScreen() {
            _uiDocument.enabled = true;
            _textureController.EnableRendering();
        }

        private void HideLoadingScreen() {
            _uiDocument.enabled = false;
            _textureController.DisableRendering();
        }
    }
}
