﻿using EasyTransition;
using FH.SO;
using FH.Utils;
using NaughtyAttributes;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Localization.Settings;
using FH.Sound;
using PlatformFeatures;
using PlatformFeatures.AdFeatures;
using PlatformFeatures.MetrikaFeatures;
using PlatformFeatures.SaveFeatures;

namespace FH.Init {
    public class InitialSceneController : MonoBehaviour {
        [Header("Registred Scenes")]
        [Scene] public string mainMenuScene;
        [Scene] public string levelScene;

        [Header("System Referenses")]
        public GameContext gameContext;
        public SettingsSO settings;

        [Header("Level References")]
        public SceneTransitionManager sceneTransitionManager;
        public UIDocument uiDocument;

        protected readonly SettingsObserver _settingsObserver = new();
        private bool _isLoading = false;

        protected virtual void OnTransitionIn() { }
        protected virtual void OnTransitionOut() { }

        private void Start() {
            ShowLoadingScreen();

            gameContext.SceneManagerProxy.IsManaged = true;
            gameContext.SceneManagerProxy.MainMenuTransitionRequested += () => { if (!_isLoading) LoadMainMenuScene(); };
            gameContext.SceneManagerProxy.LevelTransitionRequested += () => { if (!_isLoading) LoadLevelScene(); };

            _ = InitGame();
        }

        private async Awaitable InitGame() {
            // Init game here

            await SaveFeatures.Instance.LoadDataAwaitable(5);
            await LocalizationSettings.InitializationOperation.CompleteAsync();

            // Set current language definded by unity
            settings.SfxVolume = SaveFeatures.Instance.SaveInfo.SfxVolume;
            settings.MusicVolume = SaveFeatures.Instance.SaveInfo.MusicVolume;
            settings.LocaleIdentifier = new LocaleIdentifier(SaveFeatures.Instance.SaveInfo.Language);
            
            await _settingsObserver.Init(settings);

            int index = 1;
            foreach (var level in gameContext.LevelDataBase.Levels) {
                level.number = index++;
            }
            
#if !UNITY_EDITOR
            var data = SaveFeatures.Instance.SaveInfo.LevelsScore;
            foreach (var level in gameContext.LevelDataBase.Levels) {
                if (data.TryGetValue(level.number, out float levelScore)) {
                    level.score = levelScore;
                    level.isCompleted = true;
                }
                else {
                    level.score = 0;
                    level.isCompleted = false;
                }
            }
#endif

            // Load Main Menu
            await LoadMainMenuScene();
        }

        private Awaitable LoadMainMenuScene() {
            return LoadScene(mainMenuScene, false);
        }

        private Awaitable LoadLevelScene() {
            return LoadScene(levelScene, true);
        }

        private async Awaitable LoadScene(string sceneName, bool showAd) {
            if (_isLoading)
                return;

            _isLoading = true;

            if (gameContext.SceneManagerProxy.SceneController != null) {
                await ExitCurrentScene();
                await Awaitable.WaitForSecondsAsync(2f);
            }

            if (showAd) {
                await AdFeatures.Instance.ShowFullscreenAwaitable();
            }

            await LoadNewScene(sceneName);
        }

        private async Awaitable LoadNewScene(string sceneName) {
            gameContext.SceneManagerProxy.SceneControllerSet += OnControllerSet;

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
            await sceneTransitionManager.StartTransition();
            ShowLoadingScreen();

            try {
                await gameContext.SceneManagerProxy.SceneController.UnloadScene();
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
        }

        private void OnControllerSet() {
            gameContext.SceneManagerProxy.SceneControllerSet -= OnControllerSet;
            _ = OnControllerSetAsync();
        }

        private async Awaitable OnControllerSetAsync() {
            await PrepareScene();
            await Awaitable.NextFrameAsync();
            await EnterScene();
            _isLoading = false;
        }

        private Awaitable PrepareScene() {
            return gameContext.SceneManagerProxy.SceneController.StartPreloading();
        }

        private async Awaitable EnterScene() {
            await sceneTransitionManager.StartTransition();

            MetrikaFeatures.Instance.SendGameReady();
            MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.GameLoaded);
            
            HideLoadingScreen();

            MetrikaFeatures.Instance.SendGameReady();
            gameContext.SceneManagerProxy.SceneController.StartScene();

        }

        private void ShowLoadingScreen() {
            uiDocument.enabled = true;
            ScrollingBgTextureController.Instance.EnableRendering();
        }

        private void HideLoadingScreen() {
            uiDocument.enabled = false;
            ScrollingBgTextureController.Instance.DisableRendering();

        }
    }
}
