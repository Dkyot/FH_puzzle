using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static FH.UI.Rang;

namespace FH.UI.Views.LevelCompleted {
    public class LevelCompletedView : ViewBase {
        private const string _transitionClass = "transition";

        public event Action ToMainMenuPressed {
            add => _toMainMenuButton.clicked += value;
            remove => _toMainMenuButton.clicked -= value;
        }

        public event Action NexLevelPressed {
            add => _continueButton.clicked += value;
            remove => _continueButton.clicked -= value;
        }

        public event Action RateGamePressed {
            add => _rateGameButton.clicked += value;
            remove => _rateGameButton.clicked -= value;
        }

        public string TimeLabelText {
            set => _timeSoreLabel.text = value;
        }

        public string MistakesLabelText {
            set => _mistakesScoreLabel.text = value;
        }

        public string TotalScoreLableText {
            set => _totalScoreLabel.text = value;
        }

        public RangTypes Rang {
            set => _rang.RangType = value;
        }

        private Rang _rang;
        private PhotoCard _photo;

        private Label _timeSoreLabel;
        private Label _mistakesScoreLabel;
        private Label _totalScoreLabel;

        private LocalizedLabel _title;
        private LocalizedLabel _pressToContinueLabel;

        private VisualElement _viewBackground;
        private VisualElement _contentContainer;

        private VisualElement _flashScreen;

        private Button _continueButton;
        private Button _toMainMenuButton;
        private Button _rateGameButton;

        public override void Show() {
            base.Show();

            HidePressToContinueLabel();
            ResetFlashAnimation();
            ResetTitleAnimation();
            ResetContentAnimation();
            _rang.ResetAnimation();
            _photo.ResetAnimation();
        }

        public override void Hide() {
            base.Hide();
            ResetTitleAnimation();
            ResetContentAnimation();
        }

        public void ShowNextLevelButton() => _continueButton.style.display = DisplayStyle.Flex;
        public void HideNextLevelButton() => _continueButton.style.display = DisplayStyle.None;
        public void ShowRateGameButton() => _rateGameButton.style.display = DisplayStyle.Flex;
        public void HideRateGameButton() => _rateGameButton.style.display = DisplayStyle.None;

        public async Awaitable ShowFlash() {
            StartFlashAnimation();
            _flashScreen.style.display = DisplayStyle.Flex;
            await Awaitable.WaitForSecondsAsync(0.5f);
        }

        public async Awaitable HideFlash() {
            ResetFlashAnimation();
            await Awaitable.WaitForSecondsAsync(0.5f);
        }

        public void ShowTitle() {
            StartTitleAnimation();
        }

        public void ShowPressToContinueLabel() {
            _pressToContinueLabel.style.display = DisplayStyle.Flex;
        }

        public void HidePressToContinueLabel() {
            _pressToContinueLabel.style.display = DisplayStyle.None;
        }

        public async Awaitable ShowContent() {
            StartContentAnimation();
            await Awaitable.WaitForSecondsAsync(0.2f);
            _photo.StartAnimation();
        }

        public void ShowRang() {
            _rang.StartAnimation();
        }

        public void SetImage(Sprite sprite) {
            _photo.SetImage(sprite);
        }

        protected override void OnInit() {
            _rang = this.Q<Rang>("Rang");
            _photo = this.Q<PhotoCard>("Photo");

            _viewBackground = this.Q<VisualElement>("ViewBackground");
            _contentContainer = this.Q<VisualElement>("ContentContainer");

            _title = this.Q<LocalizedLabel>("Title");
            _pressToContinueLabel = this.Q<LocalizedLabel>("PressToContinueLabel");

            _timeSoreLabel = this.Q<Label>("TimeValue");
            _totalScoreLabel = this.Q<Label>("TotalValue");
            _mistakesScoreLabel = this.Q<Label>("MistakesValue");

            _continueButton = this.Q<Button>("ContinueButton");
            _toMainMenuButton = this.Q<Button>("ToMainMenuButton");
            _rateGameButton = this.Q<Button>("RateGameButton");

            _continueButton.clicked += InvokeButtonPressed;
            _toMainMenuButton.clicked += InvokeButtonPressed;
            _rateGameButton.clicked += InvokeButtonPressed;
            _continueButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _toMainMenuButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _rateGameButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);

            _flashScreen = this.Q("FlashScreen");

            ResetFlashAnimation();
            ResetTitleAnimation();
            ResetContentAnimation();
        }

        private void StartFlashAnimation() {
            _flashScreen.RemoveFromClassList(_transitionClass);
        }

        private void ResetFlashAnimation() {
            _flashScreen.AddToClassList(_transitionClass);
        }

        private void StartTitleAnimation() {
            _title.RemoveFromClassList(_transitionClass);
        }

        private void ResetTitleAnimation() {
            _title.AddToClassList(_transitionClass);
        }

        private void StartContentAnimation() {
            _contentContainer.style.display = DisplayStyle.Flex;
            _contentContainer.RemoveFromClassList(_transitionClass);
            _viewBackground?.RemoveFromClassList(_transitionClass);
        }

        private void ResetContentAnimation() {
            _contentContainer.style.display = DisplayStyle.None;
            _contentContainer.AddToClassList(_transitionClass);
            _viewBackground?.AddToClassList(_transitionClass);
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelCompletedView, UxmlTraits> { }
    }
}