using System;
using System.Globalization;
using FH.SO;
using PlatformFeatures.SaveFeatures;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace FH.Init {
    public sealed class SettingsObserver {
       private SettingsSO _settings;

        public async Awaitable Init(SettingsSO settingsSO) {
            _settings = settingsSO;
            _settings.MusicVolumeChanged += OnMusicVolumeChanged;
            _settings.SfxVolumeChanged += OnSfxVolumeChanged;
            _settings.LocaleChanged += OnLocaleChanged;
            OnLocaleChanged();
        }

        private void OnMusicVolumeChanged()
        {
            SaveFeatures.Instance.SaveInfo.MusicVolume = _settings.MusicVolume;
        }

        private void OnSfxVolumeChanged()
        {
            SaveFeatures.Instance.SaveInfo.SfxVolume = _settings.SfxVolume;
        }

        private void OnLocaleChanged() {
            try {
                var newLocale = LocalizationSettings.AvailableLocales.GetLocale(_settings.LocaleIdentifier);
                LocalizationSettings.SelectedLocale = newLocale;
            }
            catch (Exception e) {
                Debug.Log(e);
            }
            
        }
    }
}