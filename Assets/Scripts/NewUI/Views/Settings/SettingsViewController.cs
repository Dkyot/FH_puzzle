using System;
using FH.SO;
using FH.Sound;
using FH.Utils;
using UnityEngine;
using UnityEngine.Localization;
using YandexSDK.Scripts;

namespace FH.UI.Views.Settings {
    public sealed class SettingsViewController : ViewController<SettingsView> {
        [SerializeField] private ViewController _viewAfter;

        [SerializeField] private SettingsSO _settings;
        [SerializeField] private LocalizationOption[] _avalibleLanguages;

        [Header("Sounds")]
        [SerializeField] private AudioClip _sfxChangeSound;

        private int _currentLanguageIndex;

        public override void ShowView() {
            ScrollingBgTextureController.Instance?.EnableRendering();
            base.ShowView();
            YandexGamesManager.CallYandexMetric("SettingsOpened");
        }

        public override void HideView() {
            ScrollingBgTextureController.Instance?.DisableRendering();
            base.HideView();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();

            view.SetMusicValue(_settings.MusicVolume);
            view.SetSfxValue(_settings.SfxVolume);

            view.DonePressed += OnDonePressed;

            view.LanguageLeftSwitched += OnLanguageChangedLeft;
            view.LanguageRightSwitched += OnLanguageChangedRight;

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
            LocalYandexData.Instance.SaveData();
        }

        private void OnLanguageChangedLeft() {
            _currentLanguageIndex--;
            if (_currentLanguageIndex < 0) {
                _currentLanguageIndex = _avalibleLanguages.Length - 1;
            }

            ChangeLanguage();
        }

        private void OnLanguageChangedRight() {
            _currentLanguageIndex++;
            if (_currentLanguageIndex >= _avalibleLanguages.Length) {
                _currentLanguageIndex = 0;
            }

            ChangeLanguage();
        }
        private void Start() {
            SetCurrentLanguageIndex();
            view.SetLanguageNameKey(_avalibleLanguages[_currentLanguageIndex].localizationNameKey);
        }

        private void ChangeLanguage() {
            var option = _avalibleLanguages[_currentLanguageIndex];
            view.SetLanguageNameKey(option.localizationNameKey);
            _settings.LocaleIdentifier = option.localeIdentifier;
        }

        private void OnDisable() {
            view.DonePressed -= OnDonePressed;
            view.LanguageLeftSwitched -= OnLanguageChangedLeft;
            view.LanguageRightSwitched -= OnLanguageChangedRight;
        }

        private void SetCurrentLanguageIndex() {
            var locale = _settings.LocaleIdentifier;

            int index = -1;
            for (int o = 0; o < _avalibleLanguages.Length; o++) {
                LocalizationOption item = _avalibleLanguages[o];
                if (locale != item.localeIdentifier)
                    continue;

                index = o;
                break;
            }

            if (index < 0) {
                Debug.LogError("Current language is not in avaliabe locale list!!!");

                _currentLanguageIndex = 0;
                _settings.LocaleIdentifier = _avalibleLanguages[_currentLanguageIndex].localeIdentifier;
            }
            else {
                _currentLanguageIndex = index;
            }
        }

        [Serializable]
        public sealed class LocalizationOption {
            public string localizationNameKey;
            public LocaleIdentifier localeIdentifier;
        }
    }

}