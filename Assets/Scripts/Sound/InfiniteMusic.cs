using FH.SO;
using UnityEngine;

namespace FH.Sound
{
    public sealed class InfiniteMusic : MonoBehaviour
    {
        [SerializeField] private SettingsSO settings;
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            settings.MusicVolumeChanged += OnMusicVolumeChanged;
            OnMusicVolumeChanged();
        }

        private void OnMusicVolumeChanged()
        {
            audioSource.volume = settings.MusicVolume;
        }
    }
}