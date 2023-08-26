using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.MainMenu {
    public sealed class MainMenuViewController : ViewController {
        private MainMenuView _mainMenu;

        [SerializeField] private ViewController _settingsView;
        [SerializeField] private ViewController _playView;

        public override void ShowView() {
            _mainMenu.Show();
        }

        public override void HideView() {
            _mainMenu.Hide();
        }

        protected override void OnScreenControllerSet() {
            _mainMenu = ScreenController.Document.rootVisualElement.Q<MainMenuView>();
            _mainMenu.Init();
            _mainMenu.SettingsPressed += OnSettingPressed;
            _mainMenu.PlayPressed += OnPlayPressed;
        }

        private void OnPlayPressed() {
            ScreenController.ShowView(_playView);
        }

        private void OnSettingPressed() {
            ScreenController.ShowView(_settingsView);
        }

        private void OnDisable() {
            _mainMenu.SettingsPressed -= OnSettingPressed;
            _mainMenu.PlayPressed -= OnPlayPressed;
        }
    }
}