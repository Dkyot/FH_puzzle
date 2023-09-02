using System.Collections;
using System.Collections.Generic;
using FH.Inputs;
using FH.Level;
using FH.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelCompleted {
    public sealed class LevelCompletedController : ViewController<LevelCompletedView> {
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private ScoreCounter _scoreCounter;

        public void ShowContent() {
            _playerInputHandler.Pressed -= OnPressed;
            view.ShowContent();
        }

        public override void ShowView() {
            _bgTextureController.EnableRendering();
            view.Show();
            view.ShowTitle();

            _playerInputHandler.Pressed += OnPressed;
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
            view.Hide();
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
        }

        private void OnPressed(Vector2 position) {
            ShowContent();
        }

        private void OnDisable() {
        }
    }
}