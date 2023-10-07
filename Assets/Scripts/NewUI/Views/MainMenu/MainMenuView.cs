using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.MainMenu {
    public sealed class MainMenuView : ViewBase  {
        public event Action PlayPressed {
            add => _playButton.clicked += value;
            remove => _playButton.clicked -= value;
        }

        public event Action GalleryPressed {
            add => _galleryButton.clicked += value;
            remove => _galleryButton.clicked -= value;
        }

        public event Action SettingsPressed {
            add => _settingsButton.clicked += value;
            remove => _settingsButton.clicked -= value;
        }

        private Button _playButton;
        private Button _galleryButton;
        private Button _settingsButton;
        private VisualElement _buttonsContainer;

        public MainMenuView() {
        }

        protected override void OnInit() {
            var menuContainer = this.Q<VisualElement>("Menu");
            _buttonsContainer = menuContainer.Q<VisualElement>("ButtonsContainer");

            _playButton = _buttonsContainer.Q<Button>("PlayButton");
            _galleryButton = _buttonsContainer.Q<Button>("GalleryButton");
            _settingsButton = _buttonsContainer.Q<Button>("SettingsButton");

            _playButton.clicked += InvokeButtonPressed;
            _galleryButton.clicked+= InvokeButtonPressed;
            _settingsButton.clicked+= InvokeButtonPressed;
            _playButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _galleryButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _settingsButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        public new sealed class UxmlFactory : UxmlFactory<MainMenuView, UxmlTraits> { }
    }
}