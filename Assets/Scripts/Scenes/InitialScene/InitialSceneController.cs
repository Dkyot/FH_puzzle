using EasyTransition;
using FH.SO;
using FH.Utils;
using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Assets.Scripts.InitialScene {
    public sealed class InitialSceneController : MonoBehaviour {
        [Header("Registred Scenes")]
        [SerializeField, Scene] private string _mainMenuScene;
        [SerializeField, Scene] private string _levelScene;

        [Header("System Referenses")]
        [SerializeField] private SceneManagerProxy _sceneManagerProxy;
        [SerializeField] private LevelsDataBaseSO _levelDataBase;

        [Header("Level References")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private TransitionSettings _transitionSettings;
        [SerializeField] private ScrollingBgTextureController _textureController;

        private bool _isLoading = false;

        private void Start() {
            ShowLoadingScreen();

            _sceneManagerProxy.IsManaged = true;
            _sceneManagerProxy.MainMenuTransitionRequested += () => { if (!_isLoading) LoadMainMenuScene(); };
            _sceneManagerProxy.LevelTransitionRequested += () => { if (!_isLoading) LoadLevelScene(); };

            _ = InitGame();
        }

        private async Awaitable InitGame() {
            // Init game here
            // Load saved player level data

            // Indexing levels
            var levels = _levelDataBase.LevelData.Select((l, i) => {
                l.number = i + 1;
                return l;
            });

            foreach (var level in levels) { }

            // Load Mian Menu
            await Awaitable.WaitForSecondsAsync(2);
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

            if (_sceneManagerProxy.SceneController != null) {
                await ExitCurrentScene();
                await Awaitable.WaitForSecondsAsync(2f);
            }

            if (showAd) {
                // Show Ad Here
            }

            await LoadNewScene(sceneName);
        }

        private async Awaitable LoadNewScene(string sceneName) {
            _sceneManagerProxy.SceneControllerSet += OnControllerSet;

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
                await _sceneManagerProxy.SceneController.UnloadScene();
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private void OnControllerSet() {
            _sceneManagerProxy.SceneControllerSet -= OnControllerSet;
            _ = OnControllerSetAsync();
        }

        private async Awaitable OnControllerSetAsync() {
            await PrepareScene();
            await Awaitable.NextFrameAsync();
            await EnterScene();
            _isLoading = false;
        }

        private async Awaitable PrepareScene() {
            await _sceneManagerProxy.SceneController.StartPreloading();
        }

        private async Awaitable EnterScene() {
            await StartTransition();
            HideLoadingScreen();
            _sceneManagerProxy.SceneController.StartScene();
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
