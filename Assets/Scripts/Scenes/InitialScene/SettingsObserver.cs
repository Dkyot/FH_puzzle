using FH.SO;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace FH.Init {
    public sealed class SettingsObserver {
       private SettingsSO _settings;

        public async Awaitable Init(SettingsSO settingsSO) {
            _settings = settingsSO;

            // TODO Load saved user settings 

            _settings.MusicVolumeChanged += OnMusicVolumeChanged;
            _settings.SfxVolumeChanged += OnSfxVolumeChanged;
            _settings.LocaleChanged += OnLocaleCHanged;
        }

        private void OnMusicVolumeChanged() {
            // TODO Save Music volume
        }

        private void OnSfxVolumeChanged() {
            // TODO Save Sfx Volume
        }

        private void OnLocaleCHanged() {
            var newLocale = LocalizationSettings.AvailableLocales.GetLocale(_settings.LocaleIdentifier);
            LocalizationSettings.SelectedLocale = newLocale;

            // TODO Save locale code
        }
    }
}