using System;
using System.Collections;
using System.Collections.Generic;
using FH.Inputs;
using FH.Level;
using FH.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController<LevelCompletedView> {
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private ScoreCounter _scoreCounter;

        public void ShowContent() {
            view.ShowContent();
            _ = StartScoreAnimation();
        }

        public override void ShowView() {
            _bgTextureController.EnableRendering();
            view.Show();
            view.ShowTitle();
            _ = AwaitTitleAnimation();

        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            view.Hide();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
        }

        private async Awaitable AwaitTitleAnimation() {
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

            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentMinutes = (int)math.lerp(0, minutes, step);
                int currentSeconds = (int)math.lerp(0, seconds, step);
                int currentMSeconds = (int)math.lerp(0, mseconds, step);
                view.TimeLabelText = $"{currentMinutes:00}:{currentSeconds:00}:{currentMSeconds:000}";
            }
        }

        private async Awaitable StartMistakesAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            int mistakes = _scoreCounter.Mismatch;

            const float stepSpeed = 1f;
            float step = 0f;

            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentValue = (int)math.lerp(0, mistakes, step);
                view.MistakesLabelText = currentValue.ToString();
            }
        }

        private async Awaitable StartTotalScoreAnimation() {
            var cancellationToken = Application.exitCancellationToken;
            float totalScore = _scoreCounter.FinalScore;

            const float stepSpeed = 1f;
            float step = 0f;

            while (!cancellationToken.IsCancellationRequested && step < 1) {
                await Awaitable.NextFrameAsync();
                step = math.clamp(step + stepSpeed * Time.deltaTime, 0, 1);
                int currentValue = (int)math.lerp(0, totalScore, step);
                view.TotalScoreLableText = currentValue.ToString();
            }
        }

        private void ShowContentOnPressed(Vector2 position) {
            ShowContent();
            _playerInputHandler.Pressed -= ShowContentOnPressed;
        }

        private void OnDisable() {
        }
    }
}