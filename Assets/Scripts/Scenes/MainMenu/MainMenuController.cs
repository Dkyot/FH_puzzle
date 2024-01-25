using FH.Scenes;
using FH.SO;
using FH.Sound;
using FH.UI.Views.Gallery;
using FH.UI.Views.LevelSelect;
using FH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace FH.MainMenu {
    public class MainMenuController : MonoBehaviour, ISceneController {
        [Header("System Referenses")]
        [SerializeField] private GameContext _gameContext;

        [Header("SceneReferences")]
        [SerializeField] private GalleryViewController _galleryController;
        [SerializeField] private LevelSelectController _levelSelectController;

        [Header("Music")]
        [SerializeField] private AudioClip _music;

        private List<Sprite> _galleryImages = new();

        public void GoToLevel(AddressableLevelDataSO levelData) {
            _gameContext.CurrentLevel = levelData;
            _gameContext.SceneManagerProxy.RequestLevelTransition();
        }

        public async Awaitable StartPreloading() {
            try {
                await LoadGalleryImages();
                _galleryController.SetImages(_galleryImages);
            }
            catch (Exception ex) {
                Debug.LogException(ex);
            }

            _levelSelectController.SetLevels(_gameContext.LevelDataBase.Levels);
        }

        public void StartScene() {
            MusicManager.Instance?.FadeIn(0.5f, _music, true);
        }

        public async Awaitable UnloadScene() {
            await MusicManager.Instance?.FadeOut(0.3f);
            RealeseImages();
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        private void Start() {
            _gameContext.SceneManagerProxy.SceneController = this;
        }

        private async Awaitable LoadGalleryImages() {
            var comopletedLevels = _gameContext.LevelDataBase.Levels.Where(l => l.isCompleted);

            foreach (var level in comopletedLevels) {
                var levelImageRef = level.LevelImage;
                AsyncOperationHandle<Sprite> loadOperation;

                try {
                    loadOperation = await levelImageRef.LoadAssetAsync().CompleteAsync();
                }
                catch (Exception ex) {
                    Debug.LogException(ex);
                    continue;
                }

                if (loadOperation.Status == AsyncOperationStatus.Failed) {
                    Debug.Log(loadOperation.OperationException);
                    continue;
                }

                var result = loadOperation.Result;
                if (result == null) {
                    Debug.Log($"Loaded image is null: {levelImageRef.AssetGUID}");
                    continue;
                }

                _galleryImages.Add(result);
            }

            Debug.Log($"Loaded {_galleryImages.Count} images");
        }

        private void RealeseImages() {
            _galleryImages.Clear();
            _galleryImages = null;

            var comopletedLevels = _gameContext.LevelDataBase.Levels.Where(l => l.isCompleted);

            foreach (var level in comopletedLevels) {
                var levelImageRef = level.LevelImage;
                levelImageRef.ReleaseAsset();
            }
        }
    }
}
