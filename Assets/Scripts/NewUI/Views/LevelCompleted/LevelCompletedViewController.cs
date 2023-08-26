using System.Collections;
using System.Collections.Generic;
using FH.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController {
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        [SerializeField] private ViewController _viewOnBack;
        private LevelCompletedView _levelCompletedView;
        
        public override void ShowView() {
            _levelCompletedView.BackButtonPressed += OnBackPressed;
            _bgTextureController.EnableRendering();
            _levelCompletedView.Show();
        }

        public override void HideView() {
            _levelCompletedView.BackButtonPressed -= OnBackPressed;
            _bgTextureController.DisableRendering();
            _levelCompletedView.Hide();
        }

        protected override void OnScreenControllerSet() {
            _levelCompletedView = ScreenController.Document.rootVisualElement.Q<LevelCompletedView>();
            _levelCompletedView.Init();
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }
    }
}