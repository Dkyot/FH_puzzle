using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.MainMenu {
    public sealed class MainMenuContainer : VisualElement, IMainMenu, IView {
        public event Action playPressed {
            add => _playButton.clicked += value;
            remove => _playButton.clicked -= value;
        }

        public event Action galleryPressed {
            add => _galleryButton.clicked += value;
            remove => _galleryButton.clicked -= value;
        }

        public event Action settingsPressed {
            add => _settingsButton.clicked += value;
            remove => _settingsButton.clicked -= value;
        }

        private Button _playButton;
        private Button _galleryButton;
        private Button _settingsButton;
        private VisualElement _buttonsContainer;

        public MainMenuContainer() {
        }

        public void Init() {
            var menuContainer = this.Q<VisualElement>("Menu");
            _buttonsContainer = menuContainer.Q<VisualElement>("ButtonsContainer");

            _playButton = _buttonsContainer.Q<Button>("PlayButton");
            _galleryButton = _buttonsContainer.Q<Button>("GalleryButton");
            _settingsButton = _buttonsContainer.Q<Button>("SettingsButton");
        }

        public void Activate() {
            _playButton.Focus();
        }

        public void Disable() {
        }

        public new sealed class UxmlFactory : UxmlFactory<MainMenuContainer, UxmlTraits> { }
    }
}