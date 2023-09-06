using EasyTransition;
using FH.SO;
using FH.Utils;
using NaughtyAttributes;
using System;
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

        [Header("Level References")]
        [SerializeField] private Camera _sceneCamera;
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private TransitionSettings _transitionSettings;
        [SerializeField] private ScrollingBgTextureController _textureController;

        private bool _isLoading = false;

        private void Start() {
            ShowScene();
            _sceneManagerProxy.IsManaged = true;
            _ = InitGame();
        }

        private async Awaitable InitGame() {
            // Init game here

            await Awaitable.WaitForSecondsAsync(1);
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
                var scene = SceneManager.GetSceneByName(sceneName);
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
            TransitionManager.Instance().Transition(_transitionSettings, 0);
            await Awaitable.WaitForSecondsAsync(1f);
            HideScene();
            _sceneManagerProxy.SceneController.StartScene();
        }

        private Awaitable LoadMainMenuScene() {
            return LoadScene(_mainMenuScene);
        }

        private Awaitable LoadLevelScene() {
            return LoadScene(_levelScene);
        }

        private void ShowScene() {
            _sceneCamera.enabled = true;
            _uiDocument.enabled = true;
            _textureController.EnableRendering();
        }

        private void HideScene() {
            _sceneCamera.enabled = false;
            _uiDocument.enabled = false;
            _textureController.DisableRendering();
        }
    }
}
