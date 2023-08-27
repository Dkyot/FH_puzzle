using System;
using FH.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Settings {
    public sealed class SettingsViewController : ViewController<SettingsView> {
        [SerializeField] private ViewController _viewAfter;
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        
        public override void ShowView() {
            _bgTextureController.EnableRendering();
            base.ShowView();
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            base.HideView();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.DonePressed += OnDonePressed;
        }

        private void OnDonePressed() {
            ScreenController.ShowView(_viewAfter);
        }

        private void OnDisable() {
            view.DonePressed -= OnDonePressed;
        }
    }
}