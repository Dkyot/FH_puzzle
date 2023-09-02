using FH.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectController : ViewController<LevelSelectView> {
        [SerializeField] private ViewController _viewOnBack;

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackPressed += OnBackPressed;
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnLevelSelected() {
        }

        private void OnDisable() {
            view.BackPressed -= OnBackPressed;
        }
    }
}
