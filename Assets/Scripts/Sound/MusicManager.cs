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
            _audioSource.Play();
        }

        public void Stop() {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        public Awaitable FadeIn(float time, AudioClip music, bool loop = true) {
            _audioSource.volume = 0;
            _volume = 0;
            Play(music, loop);
            return Fade(time, _settings.MusicVolume);
        }

        public async Awaitable FadeOut(float time) {
            await Fade(time, 0);
            _audioSource.Stop();
        }

        private async Awaitable Fade(float time, float destination) {
            var cancellToken = Application.exitCancellationToken;

            float initialVolume = _volume;
            float fadeSpeed = Mathf.Abs((destination - initialVolume) / time);

            if (fadeSpeed == 0)
                return;

            float t = 0;

            while (!cancellToken.IsCancellationRequested && t < 1) {
                await Awaitable.NextFrameAsync();
                t = Mathf.Clamp(t + fadeSpeed * Time.deltaTime, 0f, 1f);
                float newVolume = Mathf.Lerp(initialVolume, destination, t);
                Volume = newVolume;
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