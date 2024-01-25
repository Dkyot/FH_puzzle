using Assets.Scripts.Utils;
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

        private readonly LevelDataImageLoader _levelDataLoader = new LevelDataImageLoader();

        private List<LevelDataSO> _completedLevels;
        private List<Sprite> _galleryImages;

        public void GoToLevel(LevelDataSO levelData) {
            _gameContext.CurrentLevel = levelData;
            _gameContext.SceneManagerProxy.RequestLevelTransition();
        }

        public async Awaitable StartPreloading() {
            _completedLevels = _gameContext.LevelDataBase.Levels.Where(l => l.isCompleted).ToList();

            try {
                await LoadGalleryImages();
                if (_galleryImages != null)
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
            if (_completedLevels.Count == 0)return;
            _galleryImages = await _levelDataLoader.LoadLevelsImages(_completedLevels);
            Debug.Log($"Loaded {_galleryImages.Count} images");
        }

        private void RealeseImages() {
            _galleryImages = null;

            foreach (var level in _completedLevels) {
                level.ReleaseImage();
            }
        }
    }
}
