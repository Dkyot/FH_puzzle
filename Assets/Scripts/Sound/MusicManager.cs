using FH.SO;
using UnityEngine;

namespace FH.Sound {
    public sealed class MusicManager : MonoBehaviour {
        public static MusicManager Instance { get; private set; }

        public float Volume {
            get => _volume;
            private set {
                SetVolume(value);
            }
        }

        [SerializeField] private SettingsSO _settings;
        private AudioSource _audioSource;

        private float _volume = 1f;

        public void Play(AudioClip music, bool loop = true) {
            _audioSource.clip = music;
            _audioSource.loop = loop;
        }

        public void Stop() {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        public Awaitable FadeIn(float time) {
            return Fade(time, _settings.MusicVolume);
        }

        public Awaitable FadeOut(float time) {
            return Fade(time, 0);
        }

        private async Awaitable Fade(float time, float destination) {
            var cancellToken = Application.exitCancellationToken;

            float initialVolume = _volume;
            float fadeSpeed = (destination - initialVolume) / time;

            if (fadeSpeed == 0)
                return;

            float t = 0;

            while (!cancellToken.IsCancellationRequested) {
                await Awaitable.NextFrameAsync();
                float newVolume = Mathf.Lerp(initialVolume, 0f, t);
                Volume = newVolume;

                t = Mathf.Clamp(t + fadeSpeed * Time.deltaTime, 0f, 1f);
            }
        }

        private void SetVolume(float volume) {
            _volume = volume;
            _audioSource.volume = volume;
        }

        private void OnVolumeChanged() {
            Volume = _settings.MusicVolume;
        }

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
                return;
            }

            Instance = this;
            CreateSource();
            Volume = _settings.MusicVolume;
            _settings.MusicVolumeChanged += OnVolumeChanged;
        }

        private void CreateSource() {
            var source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            _audioSource = source;
        }
    }
}