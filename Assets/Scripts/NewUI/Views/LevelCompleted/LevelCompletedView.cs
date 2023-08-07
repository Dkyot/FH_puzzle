using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedView : ViewBase {
        private const string _rangTransitionClass = "rang-transition";
        private const string _rangAnimationClass = "rang-animation";

        private const string _photoTransitionClass = "photo-transition";
        private const string _photoAnimationClass = "photo-animation";
        
        public event Action backButtonPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        private Label _rang;
        private VisualElement _photo;

        private Button _backButton;

        public override void Init() {
            _rang = this.Q<Label>("Rang");
            _photo = this.Q("Photo");
            _backButton = this.Q<Button>("BackButton");
        }

        public void Show() {
            style.display = DisplayStyle.Flex;
            schedule.Execute(() =>
            {
                _photo.RemoveFromClassList(_photoTransitionClass);
                _rang.AddToClassList(_rangAnimationClass);

                _rang.RemoveFromClassList(_rangTransitionClass);
                _photo.AddToClassList(_photoAnimationClass);
            }).ExecuteLater(100);
        }


        public void Hide() {
            style.display = DisplayStyle.None;

            _rang.RemoveFromClassList(_rangAnimationClass);
            _rang.AddToClassList(_rangTransitionClass);

            _photo.RemoveFromClassList(_photoAnimationClass);
            _photo.AddToClassList(_photoTransitionClass);
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelCompletedView, UxmlTraits> { }
    }
}