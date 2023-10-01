using System;
using System.Collections;
using System.Collections.Generic;
using FH.Inputs;
using FH.Level;
using FH.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController<LevelCompletedView> {
        [SerializeField] private UnityEvent _toMainMenuPressed;
        [SerializeField] private UnityEvent _nextLevelPressed;

        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private ScoreCounter _scoreCounter;

        [SerializeField] private AudioSource _scoreAudioSource;

        private void Awake() {
            _scoreAudioSource.Stop();
        }

        public void ShowContent() {
            view.ShowContent();
            _ = StartScoreAnimation();
        }

        public override void ShowView() {
            ScrollingBgTextureController.Instance?.EnableRendering();
            view.Show();
            _ = StartLevelCompleteAnimation();

        }

        private async Awaitable StartLevelCompleteAnimation() {
            await view.ShowFlash();
            await StartTitleAnimation();
        }

        public override void HideView() {
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
        }

        private void OnToMainMenUPressed() {
            _toMainMenuPressed.Invoke();
        }

        private void OnNexLevelPressed() {
            _nextLevelPressed.Invoke();
        }

        private async Awaitable StartTitleAnimation() {
            view.ShowTitle();
            await Awaitable.WaitForSecondsAsync(0.5f);
            _playerInputHandler.Pressed += ShowContentOnPressed;
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
            view.ShowRang();
        }

        private async Awaitable StartTimeAnimation() {
            var cancellationToken = Application.exitCancellationToken;

            var totalTime = TimeSpan.FromSeconds(_scoreCounter.Time);
            int minutes = totalTime.Minutes;
            int seconds = totalTime.Seconds;
            int mseconds = totalTime.Milliseconds;

            const float stepSpeed = 2f;
            float step = 0f;

            _scoreAudioSource.loop = true;
            _scoreAudioSource.Play();
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentMinutes = (int)math.lerp(0, minutes, step);
                int currentSeconds = (int)math.lerp(0, seconds, step);
                int currentMSeconds = (int)math.lerp(0, mseconds, step);
                view.TimeLabelText = $"{currentMinutes:00}:{currentSeconds:00}:{currentMSeconds:000}";
            }
            _scoreAudioSource.loop = false;
        }

        private async Awaitable StartMistakesAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            int mistakes = _scoreCounter.Mismatch;

            const float stepSpeed = 1f;
            float step = 0f;

            _scoreAudioSource.loop = true;
            _scoreAudioSource.Play();
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentValue = (int)math.lerp(0, mistakes, step);
                view.MistakesLabelText = currentValue.ToString();
            }
            _scoreAudioSource.loop = false;
        }

        private async Awaitable StartTotalScoreAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            float totalScore = _scoreCounter.FinalScore;

            const float stepSpeed = 1f;
            float step = 0f;

            _scoreAudioSource.loop = true;
            _scoreAudioSource.Play();
            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentValue = (int)math.lerp(0, totalScore, step);
                view.TotalScoreLableText = currentValue.ToString();
            }
            _scoreAudioSource.loop = false;
        }

        private void ShowContentOnPressed(Vector2 position) {
            ShowContent();
            _playerInputHandler.Pressed -= ShowContentOnPressed;
        }

        private void OnDisable() {
            view.NexLevelPressed -= OnNexLevelPressed;
            view.ToMainMenuPressed -= OnToMainMenUPressed;
        }
    }
}