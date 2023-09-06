using FH.SO;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.InitialScene {
    public sealed class InitialSceneController : MonoBehaviour {
        [Scene] public string _mainMenuScene;
        [Scene] public string _levelScene;

        [SerializeField] private SceneManagerProxy _sceneManagerProxy;

        private bool _isLoading = false;

        private void Start() {
            _sceneManagerProxy.IsManaged = true;
            _ = InitGame();
        }

        private async Awaitable InitGame() {
            // Init game here

            await Awaitable.NextFrameAsync();
            await LoadMainMenuScene();
        }

        private async Awaitable LoadScene(string sceneName) {
            if (_isLoading)
                return;

            _isLoading = true;

            await UnloadCurrentScene();
            await LoadNewScene(sceneName);
        }

        private async Awaitable LoadNewScene(string sceneName) {
            _sceneManagerProxy.SceneControllerSet += OnControllerSet;

            try {
                var scene = SceneManager.GetSceneByName(sceneName);
                await SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(scene);
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private async Awaitable UnloadCurrentScene() {
            if (_sceneManagerProxy.SceneController == null)
                return;

            try {
                await _sceneManagerProxy.SceneController.UnloadScene();
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private void OnControllerSet() {
            _sceneManagerProxy.SceneControllerSet -= OnControllerSet;
            _ = PrepareScene();
        }

        private async Awaitable PrepareScene() {
            await _sceneManagerProxy.SceneController.StartPreloading();
            await Awaitable.NextFrameAsync();

            _sceneManagerProxy.SceneController.StartScene();
            _isLoading = false;
        }

        private Awaitable LoadMainMenuScene() {
            return LoadScene(_mainMenuScene);
        }

        private Awaitable LoadLevelScene() {
            return LoadScene(_levelScene);
        }
    }
}
