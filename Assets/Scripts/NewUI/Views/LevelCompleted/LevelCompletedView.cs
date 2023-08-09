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
        
        public event Action backButtonPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        private Rang _rang;
        private PhotoCard _photo;

        private Button _backButton;

        public override void Init() {
            _rang = this.Q<Rang>("Rang");
            _photo = this.Q<PhotoCard>("Photo");
            _backButton = this.Q<Button>("BackButton");
        }

        public void Show() {
            style.display = DisplayStyle.Flex;
            schedule.Execute(() => {
                _rang.StartAnimation();
                _photo.StartAnimation();
            }).ExecuteLater(100);
        }

        public void Hide() {
            style.display = DisplayStyle.None;
            
            _rang.ResetAnimation();
            _photo.ResetAnimation();
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelCompletedView, UxmlTraits> { }
    }
}