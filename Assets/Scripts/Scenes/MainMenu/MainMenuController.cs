using FH.Scenes;
using FH.SO;
using FH.UI.Views.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace FH.MainMenu {
    public class MainMenuController : MonoBehaviour, ISceneController {
        [Header("System Referenses")]
        [SerializeField] private SceneManagerProxy _sceneManagerProxy;
        [SerializeField] private LevelsDataBaseSO _levelsDataBase;

        [Header("SceneReferences")]
        [SerializeField] private GalleryViewController _galleryController;

        private List<Sprite> _galleryImages = new();

        public async Awaitable StartPreloading() {
            try {
                await LoadGalleryImages();
            }
            catch (Exception ex) {
                Debug.LogException(ex);
            }

            _galleryController.SetInages(_galleryImages);
        }

        public void StartScene() {

        }

        public async Awaitable UnloadScene() {
            RealeseImages();
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        private void Start() {
            _sceneManagerProxy.SceneController = this;
        }

        private async Awaitable LoadGalleryImages() {
            var comopletedLevels = _levelsDataBase.LevelData.Where(l => l.isCompleted);

            foreach (var level in comopletedLevels) {
                var levelImageRef = level.LevelImage;
                var loadOperation = levelImageRef.LoadAssetAsync();

                while (!loadOperation.IsDone) {
                    await Awaitable.NextFrameAsync();
                }

                if (loadOperation.Status == AsyncOperationStatus.Succeeded) {
                    var result = loadOperation.Result;

                    if (result != null)
                        _galleryImages.Add(loadOperation.Result);
                    else Debug.Log($"Loaded image is null: {levelImageRef.AssetGUID}");
                }
                else {
                    Debug.Log(loadOperation.OperationException);
                }
            }

            Debug.Log($"Loaded {_galleryImages.Count} images");
        }

        private void RealeseImages() {
            _galleryImages.Clear();
            _galleryImages = null;

            var comopletedLevels = _levelsDataBase.LevelData.Where(l => l.isCompleted);

            foreach (var level in comopletedLevels) {
                var levelImageRef = level.LevelImage;
                levelImageRef.ReleaseAsset();
            }
        }
    }
}
