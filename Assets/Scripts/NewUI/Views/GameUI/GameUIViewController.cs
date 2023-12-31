using FH.Level;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace FH.UI.Views.GameUI {
    public class GameUIViewController : ViewController<GameUIView> {
        [SerializeField] private ScoreCounter _scoreCounter;

        [Header("UI Events")]
        [SerializeField] private UnityEvent _resetPressed;
        [SerializeField] private UnityEvent _pausePressed;
        [SerializeField] private UnityEvent _peekPressed;
        [SerializeField] private UnityEvent _findPairPressed;

        public void SetFindPairUsageCount(int count) {
            view.FindPairUsageCount = count;
        }

        public void SetPeekUsegeCount(int count) {
            view.PeekUsageCount = count;
        }

        public void SetAdPeekBonus(int value) => view.AdPeekBonus = value;
        public void SetAdFinPairdBonus(int value) => view.AdFindPairBonus = value;
        

        public void ShowTipsPointer()
        {
            view.ShowTipsPointers();
        }

        public void HideTipsPointer()
        {
            view.HideTipsPointers();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.ResetPressed += OnResetPressed;
            view.PausePressed += OnPausePressed;
            view.PeekPressed += OnPeekPressed;
            view.FindPairPressed += OnFindPairPressed;
        }

        private void Start() {
            _scoreCounter.TimeChanged += OnTimeChanged;
            _scoreCounter.MismatchChanged += OnMismatchChanged;
            HideTipsPointer();
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

        private void OnPeekPressed() {
            _peekPressed.Invoke();
        }

        private void OnFindPairPressed() {
            _findPairPressed.Invoke();
        }
    }
}
