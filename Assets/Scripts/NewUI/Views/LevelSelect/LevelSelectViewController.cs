using FH.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectController : ViewController<LevelSelectView> {
        [SerializeField] private ViewController _viewOnBack;
        [SerializeField] private ViewController _viewOnLevelSelecetd;

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackPressed += OnBackPressed;
            view.LevelSelected += OnLevelSelected;
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnLevelSelected() {
            ScreenController.ShowView(_viewOnLevelSelecetd);
        }

        private void OnDisable() {
            view.BackPressed -= OnBackPressed;
            view.LevelSelected -= OnLevelSelected;
        }
    }
}
