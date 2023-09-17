using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static FH.UI.Rang;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedView : ViewBase {
        private const string _transitionClass = "transition";

        public event Action ToMainMenuPressed {
            add => _toMainMenuButton.clicked += value;
            remove => _toMainMenuButton.clicked -= value;
        }

        public event Action NexLevelPressed {
            add => _continueButton.clicked += value;
            remove => _continueButton.clicked -= value;
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

        private VisualElement _viewContainer;
        private VisualElement _contentContainer;

        private VisualElement _flashScreen;

        private Button _continueButton;
        private Button _toMainMenuButton;

        public override void Show() {
            base.Show();

            _pressToContinueLabel.style.display = DisplayStyle.None;
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

        public async Awaitable ShowFlash() {
            StartFlashAnimation();
            await Awaitable.WaitForSecondsAsync(0.5f);
            ResetFlashAnimation();
            await Awaitable.WaitForSecondsAsync(0.5f);
        }

        public void ShowTitle() {
            _pressToContinueLabel.style.display = DisplayStyle.Flex;
            StartTitleAnimation();
        }

        public async Awaitable ShowContent() {
            _pressToContinueLabel.style.display = DisplayStyle.None;
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

            _viewContainer = this.Q<VisualElement>("ViewContainer");
            _contentContainer = this.Q<VisualElement>("ContentContainer");

            _title = this.Q<LocalizedLabel>("Title");
            _pressToContinueLabel = this.Q<LocalizedLabel>("PressToContinueLabel");

            _timeSoreLabel = this.Q<Label>("TimeValue");
            _mistakesScoreLabel = this.Q<Label>("MistakesValue");
            _totalScoreLabel = this.Q<Label>("TotalValue");

            _continueButton = this.Q<Button>("ContinueButton");
            _toMainMenuButton = this.Q<Button>("ToMainMenuButton");

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
            _viewContainer.RemoveFromClassList(_transitionClass);
        }

        private void ResetContentAnimation() {
            _contentContainer.style.display = DisplayStyle.None;
            _contentContainer.AddToClassList(_transitionClass);
            _viewContainer.AddToClassList(_transitionClass);
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelCompletedView, UxmlTraits> { }
    }
}