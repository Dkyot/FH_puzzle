using System.Collections;
using System.Collections.Generic;
using FH.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController<LevelCompletedView> {
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        [SerializeField] private ViewController _viewOnBack;
        
        public override void ShowView() {
            _bgTextureController.EnableRendering();
            view.Show();
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            view.Hide();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackButtonPressed += OnBackPressed;
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnDisable() {
            view.BackButtonPressed -= OnBackPressed; 
        }
    }
}