using FH.SO;
using UnityEngine;

namespace FH.Sound {
    public sealed class SoundManager : MonoBehaviour {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private SettingsSO _settings;

        public float Volume {
            get => _volume;
            set => SetVolume(value);
        }

        private float _volume = 1.0f;

        private AudioSource _playAudioSource;
        private AudioSource _oneShotAudioSource;

        public void Play(AudioClip clip, bool loop = false) {
            _playAudioSource.volume = _volume;
            PlaySound(clip, loop);
        }

        public void Play(AudioClip clip, float volumeScale, bool loop = false) {
            _playAudioSource.volume = _volume * volumeScale;
            PlaySound(clip, loop);
        }

        public void StopLoop() {
            _playAudioSource.loop = false;
        }

        public void Stop() {
            _playAudioSource.Stop();
        }

        public void PlayOneShot(AudioClip clip) {
            _oneShotAudioSource.PlayOneShot(clip);
        }

        public void PlayOneShot(AudioClip clip, float volumeScale) {
            _oneShotAudioSource.PlayOneShot(clip, volumeScale);
        }

        private void SetVolume(float volume) {
            _volume = volume;
            _oneShotAudioSource.volume = volume;
            _playAudioSource.volume = volume;
        }

        private void PlaySound(AudioClip clip, bool loop) {
            _playAudioSource.loop = loop;
            _playAudioSource.clip = clip;
            _playAudioSource.Play();
        }

        private void OnVolumeChanged() {
            Volume = _settings.SfxVolume;
        }

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
                return;
            }

            Instance = this;

            CreateSources();

            Volume = _settings.SfxVolume;
            _settings.SfxVolumeChanged += OnVolumeChanged;
        }

        private void CreateSources() {
            var source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            _oneShotAudioSource = source;

            source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            _playAudioSource = source;
        }

        private void OnDestroy() {
            _settings.SfxVolumeChanged -= OnVolumeChanged;
            Instance = null;
        }
    }
}
