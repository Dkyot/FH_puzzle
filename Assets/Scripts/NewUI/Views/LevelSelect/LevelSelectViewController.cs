using FH.SO;
using FH.Sound;
using FH.UI;
using FH.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YandexSDK.Scripts;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectController : ViewController<LevelSelectView> {
        [SerializeField] private ViewController _viewOnBack;
        [SerializeField] private UnityEvent<LevelDataSO> LevelSelected;

        [Header("Sounds")]
        [SerializeField] private AudioClip _levelSelectedSound;
        [SerializeField, Range(0, 2)] private float _levelSelectedVolume = 1f;

        public void SetLevels(IEnumerable<LevelDataSO> levels) {
            view.SetLevels(levels);
        }

        public override void HideView() {
            base.HideView();
            ScrollingBgTextureController.Instance?.DisableRendering();
        }

        public override void ShowView() {
            base.ShowView();
            ScrollingBgTextureController.Instance?.EnableRendering();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackPressed += OnBackPressed;
            view.LevelSelected += OnLevelSelected;
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnLevelSelected(LevelDataSO level) {
            SoundManager.Instance?.PlayOneShot(_levelSelectedSound, _levelSelectedVolume);
            LevelSelected?.Invoke(level);
            YandexMetrika.LevelStarted();
        }

        private void OnDisable() {
            view.BackPressed -= OnBackPressed;
            view.LevelSelected -= OnLevelSelected;
        }
    }
}
