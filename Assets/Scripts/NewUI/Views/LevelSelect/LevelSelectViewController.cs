using FH.SO;
using FH.UI;
using FH.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FH.UI.Views.LevelSelect {
    public class LevelSelectController : ViewController<LevelSelectView> {
        [SerializeField] private ViewController _viewOnBack;
        [SerializeField] private UnityEvent<LevelDataSO> LevelSelected;

        public void SetLevels(IEnumerable<LevelDataSO> levels) {
            view.SetLevels(levels);
        }

        public override void HideView() {
            base.HideView();
            ScrollingBgTextureController.Instance.DisableRendering();
        }

        public override void ShowView() {
            base.ShowView();
            ScrollingBgTextureController.Instance.EnableRendering();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackPressed += OnBackPressed;
            view.LevelSelected += OnLevelSelected;
        }

        private void OnBackPressed() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnLevelSelected(LevelDataSO level) {
            LevelSelected?.Invoke(level);
        }

        private void OnDisable() {
            view.BackPressed -= OnBackPressed;
            view.LevelSelected -= OnLevelSelected;
        }
    }
}
