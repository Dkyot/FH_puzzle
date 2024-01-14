using System;
using FH.Inputs;
using FH.Level;
using FH.Sound;
using FH.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static FH.UI.Rang;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController<LevelCompletedView> {
        [SerializeField] private bool _toggleScrollingBgTexture = true;

        [SerializeField] private UnityEvent _toMainMenuPressed;
        [SerializeField] private UnityEvent _nextLevelPressed;
        [SerializeField] private UnityEvent _rateGamePressed;

        [SerializeField] private UnityEvent _movedToScoreTable;

        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private ScoreCounter _scoreCounter;

        [Header("Sounds")]
        [SerializeField] private AudioClip _levelCompletedSound;
        [SerializeField] private AudioClip _scoreCountSound;
        [SerializeField] private AudioClip _cameraShotSound;
        [SerializeField] private AudioClip _rangApearedSound;

        private float _scoreSoundVolume = 0.2f;
        private static bool _rateGameButtonShowed = false;

        public override void ShowView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.EnableRendering();

            view.Show();
            _ = StartLevelCompleteAnimation();
        }


        private async Awaitable StartLevelCompleteAnimation() {
            SoundManager.Instance?.PlayOneShot(_levelCompletedSound, 0.7f);
            await StartTitleAnimation();
            _playerInputHandler.Pressed += ShowContentOnPressed;
        }

        private async Awaitable ShowContent() {
            view.HidePressToContinueLabel();
            SoundManager.Instance.PlayOneShot(_cameraShotSound);
            await view.ShowFlash();
            _movedToScoreTable?.Invoke();
            await view.HideFlash();
            await view.ShowContent();
            _ = StartScoreAnimation();
        }

        public void ShowNextLevelButton() => view.ShowNextLevelButton();
        public void HideNextLevelButton() => view.HideNextLevelButton();

        public void ShowRateGameButton() {
            if (_rateGameButtonShowed)
                return;
            view.ShowRateGameButton();
        }
        public void HideRateGameButton() => view.HideRateGameButton();


        public override void HideView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.DisableRendering();

            view.Hide();
        }

        public void SetImage(Sprite sprite) {
            view.SetImage(sprite);
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.ToMainMenuPressed += OnToMainMenUPressed;
            view.NexLevelPressed += OnNexLevelPressed;
            view.RateGamePressed += OnRateGamePressed;
        }

        private void OnToMainMenUPressed() {
            _toMainMenuPressed.Invoke();
        }

        private void OnNexLevelPressed() {
            _nextLevelPressed.Invoke();
        }

        private void OnRateGamePressed() {
            _rateGameButtonShowed = true;
            _rateGamePressed?.Invoke();
            view.HideRateGameButton();
        }

        private async Awaitable StartTitleAnimation() {
            view.ShowPressToContinueLabel();
            view.ShowTitle();
            await Awaitable.WaitForSecondsAsync(0.5f);
        }

        private async Awaitable StartScoreAnimation() {
            // Await Content apearing animation
            await Awaitable.WaitForSecondsAsync(1);
            await StartTimeAnimation();
            await Awaitable.WaitForSecondsAsync(.5f);
            await StartMistakesAnimation();
            await Awaitable.WaitForSecondsAsync(.5f);
            await StartTotalScoreAnimation();
            await Awaitable.WaitForSecondsAsync(.1f);
            view.Rang = RangHelpers.CalculateRang(_scoreCounter.FinalScore);
            view.ShowRang();
            await Awaitable.WaitForSecondsAsync(0.25f);
            SoundManager.Instance?.PlayOneShot(_rangApearedSound, 0.3f);
        }

        private async Awaitable StartTimeAnimation() {
            var cancellationToken = Application.exitCancellationToken;

            var totalTime = TimeSpan.FromSeconds(_scoreCounter.Time);
            int minutes = totalTime.Minutes;
            int seconds = totalTime.Seconds;
            int mseconds = totalTime.Milliseconds;

            const float stepSpeed = 2f;
            float step = 0f;

            var soundManager = SoundManager.Instance;
            soundManager.Play(_scoreCountSound, _scoreSoundVolume, true);
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentMinutes = (int)math.lerp(0, minutes, step);
                int currentSeconds = (int)math.lerp(0, seconds, step);
                int currentMSeconds = (int)math.lerp(0, mseconds, step);
                view.TimeLabelText = $"{currentMinutes:00}:{currentSeconds:00}:{currentMSeconds:000}";
            }
            soundManager.StopLoop();
        }

        private async Awaitable StartMistakesAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            int mistakes = _scoreCounter.Mismatch;

            const float stepSpeed = 1f;
            float step = 0f;

            var soundManager = SoundManager.Instance;
            int currentValue = 0;
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int newValue = (int)math.lerp(0, mistakes, step);
                if (newValue != currentValue) {
                    currentValue = newValue;
                    view.MistakesLabelText = currentValue.ToString();
                    soundManager.PlayOneShot(_scoreCountSound, _scoreSoundVolume);
                }
            }
        }

        private async Awaitable StartTotalScoreAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            float totalScore = _scoreCounter.FinalScore;

            const float stepSpeed = 1f;
            float step = 0f;

            var soundManager = SoundManager.Instance;
            soundManager.Play(_scoreCountSound, _scoreSoundVolume, true);
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentValue = (int)math.lerp(0, totalScore, step);
                view.TotalScoreLableText = currentValue.ToString();
            }
            soundManager.StopLoop();
        }

        private void ShowContentOnPressed(Vector2 position) {
            _ = ShowContent();
            _playerInputHandler.Pressed -= ShowContentOnPressed;
        }

        private void OnDisable() {
            view.NexLevelPressed -= OnNexLevelPressed;
            view.ToMainMenuPressed -= OnToMainMenUPressed;
            view.RateGamePressed -= OnRateGamePressed;
        }
    }
}