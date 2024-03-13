using System;
using FH.SO;
using FH.Sound;
using FH.Utils;
using SDKPlatforms.Main;
using SDKPlatforms.Metrika;
using UnityEngine;
using UnityEngine.Localization;

namespace FH.UI.Views.Settings {
    public sealed class SettingsViewController : ViewController<SettingsView> {
        [SerializeField] private bool _toggleScrollingBgTexture = true;
        [SerializeField] private ViewController _viewAfter;

        [SerializeField] private SettingsSO _settings;
        [SerializeField] private LocalizationOption[] _avalibleLanguages;

        [Header("Sounds")]
        [SerializeField] private AudioClip _sfxChangeSound;

        private int _currentLanguageIndex;

        public override void ShowView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.EnableRendering();

            base.ShowView();
            PlatformFeatures.Metrika.SendEvent(MetrikaEventEnum.SettingsOpened);
        }

        public override void HideView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.DisableRendering();

            base.HideView();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();

            view.SetMusicValue(_settings.MusicVolume);
            view.SetSfxValue(_settings.SfxVolume);

            view.DonePressed += OnDonePressed;

            view.LanguageSelected += ChangeLanguage;

            view.SfxValueChanged += OnSfxVolumeChanged;
            view.MusicValueChanged += OnMusicVolumeChanged;
        }

        private void OnSfxVolumeChanged(float volume) {
            _settings.SfxVolume = volume;
            SoundManager.Instance.PlayOneShot(_sfxChangeSound, 0.05f);
        }

        private void OnMusicVolumeChanged(float volume) {
            _settings.MusicVolume = volume;
        }

        private void OnDonePressed() {
            ScreenController.ShowView(_viewAfter);
            PlatformFeatures.Save.SaveData();
        }

        private void Start() {
            view.SetLanguages(_avalibleLanguages);
        }

        private void ChangeLanguage(LocalizationOption localizationOption) {
            _settings.LocaleIdentifier = localizationOption.localeIdentifier;
        }

        private void OnDisable() {
            view.DonePressed -= OnDonePressed;
            view.LanguageSelected -= ChangeLanguage;
        }

        [Serializable]
        public sealed class LocalizationOption {
            public string localizationNameKey;
            public LocaleIdentifier localeIdentifier;
        }
    }

}