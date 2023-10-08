using System;
using UnityEngine;
using UnityEngine.Localization;
using static FH.UI.Views.Settings.SettingsViewController;

namespace FH.SO {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "SOData/Settings")]
    public sealed class SettingsSO : ScriptableObject {
        public event Action MusicVolumeChanged;
        public event Action SfxVolumeChanged;
        public event Action LocaleChanged;

        public float MusicVolume {
            get => _musicVolume;
            set {
                if (_musicVolume == value)
                    return;

                _musicVolume = value;
                MusicVolumeChanged?.Invoke();
            }
        }

        public float SfxVolume {
            get => _sfxVolume;
            set {
                if (_sfxVolume == value)
                    return;

                _sfxVolume = value;
                SfxVolumeChanged?.Invoke();
            }
        }

        public LocaleIdentifier LocaleIdentifier {
            get => _localeIndetifier;
            set {
                if (value == _localeIndetifier)
                    return;

                _localeIndetifier = value;
                LocaleChanged?.Invoke();
            }
        }

        [SerializeField] private LocaleIdentifier _localeIndetifier = "en";
        [SerializeField] private float _musicVolume = 0.5f;
        [SerializeField] private float _sfxVolume = 0.5f;
    }
}