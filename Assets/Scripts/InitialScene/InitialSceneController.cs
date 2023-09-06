using FH.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.InitialScene {
    public sealed class InitialSceneController : MonoBehaviour {
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _levelSceneName;

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
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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
            return LoadScene(_mainMenuSceneName);
        }

        private Awaitable LoadLevelScene() {
            return LoadScene(_levelSceneName);
        }
    }
}
