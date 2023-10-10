using FH.SO;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YandexSDK.Scripts;

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
            LocalYandexData.Instance.SaveInfo.MusicVolume = _settings.MusicVolume;
        }

        private void OnSfxVolumeChanged()
        {
            LocalYandexData.Instance.SaveInfo.SfxVolume = _settings.SfxVolume;
        }

        private void OnLocaleChanged() {
            var newLocale = LocalizationSettings.AvailableLocales.GetLocale(_settings.LocaleIdentifier);
            LocalizationSettings.SelectedLocale = newLocale;

            LocalYandexData.Instance.SaveInfo.Language = newLocale.Identifier.Code;
            LocalYandexData.Instance.SaveData();
        }
    }
}