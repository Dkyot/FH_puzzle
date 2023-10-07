using FH.SO;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelSelect {
    public sealed class LevelSelectView : ViewBase {
        public event Action BackPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        public event Action LevelContainerHovered;
        public event Action<LevelDataSO> LevelSelected;

        private Button _backButton;
        private VisualElement _levelsContainer;

        public void SetLevels(IEnumerable<LevelDataSO> levels) {
            _levelsContainer.Clear();

            bool isAvalible = true;

            // Avalible levels
            foreach (var level in levels) {
                var levelOption = new LevelOption {
                    LevelNumber = level.number.ToString()
                };

                if (isAvalible) {
                    levelOption.IsEnabled = true;
                    levelOption.Pressed += () => LevelSelected?.Invoke(level);
                }
                else {
                    levelOption.IsEnabled = false;
                }

                levelOption.IsCompleted = level.isCompleted;
                levelOption.RegisterCallback<MouseEnterEvent>(OnLevelContainerHovered);
                _levelsContainer.Add(levelOption);

                if (!level.isCompleted) {
                    isAvalible = false;
                }
            }
        }

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
            _levelsContainer = this.Q<VisualElement>("LevelsContainer");

            _backButton.clicked += InvokeButtonPressed;
            _backButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        private void OnLevelContainerHovered(MouseEnterEvent mouseEnterEvent) {
            LevelContainerHovered?.Invoke();
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelSelectView, UxmlTraits> { }
    }
}
