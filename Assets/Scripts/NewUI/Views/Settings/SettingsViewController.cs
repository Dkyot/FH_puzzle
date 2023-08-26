using System;
using FH.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Settings {
    public sealed class SettingsViewController : ViewController {
        [SerializeField] private ViewController _viewAfter;
        
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        private SettingsView _settingsView;
        
        public override void ShowView() {
            _bgTextureController.EnableRendering();
            _settingsView.Show();
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            _settingsView.Hide();
        }

        protected override void OnScreenControllerSet() {
            _settingsView = ScreenController.Document.rootVisualElement.Q<SettingsView>();
            _settingsView.Init();
            _settingsView.DonePressed += OnDonePressed;
        }

        private void OnDonePressed() {
            ScreenController.ShowView(_viewAfter);
        }

        private void OnDisable() {
            _settingsView.DonePressed -= OnDonePressed;
        }
    }
}