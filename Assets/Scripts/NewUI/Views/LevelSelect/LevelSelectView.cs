using System;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectView : ViewBase {
        public event Action BackPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        public event Action LevelSelected;

        private Button _backButton;
        private LevelOption _levelOption;

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
            _levelOption = this.Q<LevelOption>("Level");
            _levelOption.Pressed += (_) => LevelSelected?.Invoke();
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelSelectView, UxmlTraits> { }
    }
}
