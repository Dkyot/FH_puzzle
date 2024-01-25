using FH.SO;
using FH.Sound;
using FH.Utils;
using System.Collections.Generic;
using PlatformFeatures.MetrikaFeatures;
using UnityEngine;
using UnityEngine.Events;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectController : ViewController<LevelSelectView> {
        [SerializeField] private bool _toggleScrollingBgTexture = true;
        [SerializeField] private ViewController _viewOnBack;
        [SerializeField] private UnityEvent<AddressableLevelDataSO> LevelSelected;

        [Header("Sounds")]
        [SerializeField] private AudioClip _levelSelectedSound;
        [SerializeField, Range(0, 2)] private float _levelSelectedVolume = 1f;

        public void SetLevels(IEnumerable<AddressableLevelDataSO> levels) {
            view.SetLevels(levels);
        }

        public override void HideView() {
            base.HideView();

            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.DisableRendering();
        }

        public override void ShowView() {
            base.ShowView();

            if (_toggleScrollingBgTexture)
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

        private void OnLevelSelected(AddressableLevelDataSO level) {
            SoundManager.Instance?.PlayOneShot(_levelSelectedSound, _levelSelectedVolume);
            LevelSelected?.Invoke(level);
            MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.LevelStarted);
        }

        private void OnDisable() {
            view.BackPressed -= OnBackPressed;
            view.LevelSelected -= OnLevelSelected;
        }
    }
}
