using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.PauseMenu {
    public sealed class PauseMenuView : ViewBase {
        private const string _transitionClass = "pause-transition";

        public event Action ContinePressed {
            add => _continueButton.clicked += value;
            remove => _continueButton.clicked -= value;
        }

        public event Action ToMainMenuPressed {
            add => _toMainMenuButton.clicked += value;
            remove => _toMainMenuButton.clicked -= value;
        }

        private Button _continueButton;
        private Button _toMainMenuButton;
        private VisualElement _container;

        public override void Show() {
            base.Show();
            _container.RemoveFromClassList(_transitionClass);
            
        }

        public override void Hide() {
            base.Hide();
            _container.AddToClassList(_transitionClass);
        }

        protected override void OnInit() {
            _continueButton = this.Q<Button>("ContinueButton");
            _toMainMenuButton = this.Q<Button>("ToMainMenuButton");
            _container = this.Q<VisualElement>("ContentContainer");
        }

        public new sealed class UxmlFactory : UxmlFactory<PauseMenuView, UxmlTraits> { }
    }
}
