using System;
using System.Linq;
using FH.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace FH.UI.Views.Settings {
    public sealed class SettingsViewController : ViewController<SettingsView> {
        [SerializeField] private ViewController _viewAfter;
        [SerializeField] private ScrollingBgTextureController _bgTextureController;

        [SerializeField] private LocalizationOption[] _avalibleLanguages;

        private int _currentLanguageIndex;

        private void Start() {
            SetCurrentLanguageIndex();
            view.SetLanguageNameKey(_avalibleLanguages[_currentLanguageIndex].localizationNameKey);
        }

        public override void ShowView() {
            _bgTextureController.EnableRendering();
            base.ShowView();
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            base.HideView();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.DonePressed += OnDonePressed;
            view.LanguageLeftSwitched += OnLanguageChangedLeft;
            view.LanguageRightSwitched += OnLanguageChangedRight;
        }

        private void OnDonePressed() {
            ScreenController.ShowView(_viewAfter);
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

        private void ChangeLanguage() {
            var option = _avalibleLanguages[_currentLanguageIndex];
            var newLocale = LocalizationSettings.AvailableLocales.GetLocale(option.localeIdentifier);
            LocalizationSettings.SelectedLocale = newLocale;
            view.SetLanguageNameKey(option.localizationNameKey);
        }

        private void OnDisable() {
            view.DonePressed -= OnDonePressed;
            view.LanguageLeftSwitched -= OnLanguageChangedLeft;
            view.LanguageRightSwitched -= OnLanguageChangedRight;
        }

        private void SetCurrentLanguageIndex() {
            var locale = LocalizationSettings.SelectedLocale;
            int index = -1;
            for (int o = 0; o < _avalibleLanguages.Length; o++) {
                LocalizationOption item = _avalibleLanguages[o];
                if (locale.Identifier != item.localeIdentifier)
                    continue;

                index = o;
                break;
            }

            if (index < 0) {
                Debug.LogError("Current language is not in avaliabe locale list!!!");
                _currentLanguageIndex = 0;
                var newLocale = LocalizationSettings.AvailableLocales.GetLocale(_avalibleLanguages[_currentLanguageIndex].localeIdentifier);
                LocalizationSettings.SelectedLocale = newLocale;
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