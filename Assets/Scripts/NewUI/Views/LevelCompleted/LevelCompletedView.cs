using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedView : ViewBase {
        public event Action BackButtonPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        private Rang _rang;
        private PhotoCard _photo;
        private Button _backButton;

        public override void Show() {
            base.Show();

            schedule.Execute(() => {
                _rang.StartAnimation();
                _photo.StartAnimation();
            }).ExecuteLater(100);
        }

        public override void Hide() {
            base.Hide();
            
            _rang.ResetAnimation();
            _photo.ResetAnimation();
        }

        protected override void OnInit() {
            _rang = this.Q<Rang>("Rang");
            _photo = this.Q<PhotoCard>("Photo");
            _backButton = this.Q<Button>("BackButton");
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelCompletedView, UxmlTraits> { }
    }
}