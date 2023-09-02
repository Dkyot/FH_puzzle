using System;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelSelect {
    public sealed class LevelSelectView : ViewBase {
        public event Action BackPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        public event Action LevelSelected;

        private Button _backButton;

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelSelectView, UxmlTraits> { }
    }
}
