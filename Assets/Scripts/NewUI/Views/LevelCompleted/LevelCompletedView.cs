using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedView : ViewBase {
        private const string _transitionClass = "transition";

        private Rang _rang;
        private PhotoCard _photo;
        private LocalizedLabel _title;
        private LocalizedLabel _pressToContinueLabel;

        private VisualElement _viewContainer;
        private VisualElement _contentContainer;

        private Button _continueButton;
        private Button _toMainMenuButton;

        public override void Show() {
            base.Show();

            _rang.ResetAnimation();
            _photo.ResetAnimation();
        }

        public override void Hide() {
            base.Hide();
            ResetTitleAnimation();
            ResetContentAnimation();
        }

        public void ShowTitle() {
            _pressToContinueLabel.style.display = DisplayStyle.Flex;
            StartTitleAnimation();
        }

        public void ShowContent() {
            _pressToContinueLabel.style.display = DisplayStyle.None;
            StartContentAnimation();
            schedule.Execute(() => _photo.StartAnimation()).ExecuteLater(200);
        }

        protected override void OnInit() {
            _rang = this.Q<Rang>("Rang");
            _photo = this.Q<PhotoCard>("Photo");

            _viewContainer = this.Q<VisualElement>("ViewContainer");
            _contentContainer = this.Q<VisualElement>("ContentContainer");

            _title = this.Q<LocalizedLabel>("Title");
            _pressToContinueLabel = this.Q<LocalizedLabel>("PressToContinueLabel");

            _continueButton = this.Q<Button>("ContinueButton");
            _toMainMenuButton = this.Q<Button>("ToMainMenuButton");

            ResetTitleAnimation();
            ResetContentAnimation();
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