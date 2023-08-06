using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.MainMenu {
    public sealed class MainMenuViewController : ViewController {
        private MainMenuView _mainMenu;

        [SerializeField] private ViewController _settingsView;

        public override void ShowView() {
            _mainMenu.Activate();
        }

        public override void HideView() {
            _mainMenu.Disable();
        }

        protected override void OnScreenControllerSet() {
            _mainMenu = ScreenController.Document.rootVisualElement.Q<MainMenuView>();
            _mainMenu.Init();
            _mainMenu.settingsPressed += OnSettingPressed;
        }

        private void OnSettingPressed() {
            ScreenController.ShowView(_settingsView);
        }

        private void OnDisable() {
            _mainMenu.settingsPressed -= OnSettingPressed;
        }
    }
}