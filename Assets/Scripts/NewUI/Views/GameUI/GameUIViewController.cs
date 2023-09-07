using FH.Level;
using FH.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FH.UI.Views.GameUI {
    public class GameUIViewController : ViewController<GameUIView> {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private UnityEvent _resetPressed;
        [SerializeField] private UnityEvent _pausePressed;

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.ResetPressed += OnResetPressed;
            view.PausePressed += OnPausePressed;
        }

        private void Start() {
            _scoreCounter.TimeChanged += OnTimeChanged;
            _scoreCounter.MismatchChanged += OnMismatchChanged;
        }

        private void OnDisable() {
            view.ResetPressed -= OnResetPressed;
            view.PausePressed -= OnPausePressed;
        }

        private void OnTimeChanged(float seconds) {
            var time = TimeSpan.FromSeconds(seconds);
            view.TimeText = $"{time.Minutes:00}:{time.Seconds:00}";
        }

        private void OnMismatchChanged(int mismatch) {
            view.MistakesText = mismatch.ToString();
        }


        private void OnResetPressed() {
            _resetPressed.Invoke();
        }

        private void OnPausePressed() {
            _pausePressed.Invoke();
        }
    }
}