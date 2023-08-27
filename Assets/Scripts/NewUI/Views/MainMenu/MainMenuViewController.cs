using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.MainMenu {
    public sealed class MainMenuViewController : ViewController<MainMenuView> {
        [SerializeField] private ViewController _settingsView;
        [SerializeField] private ViewController _galleryView;
        [SerializeField] private ViewController _playView;

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.SettingsPressed += OnSettingPressed;
            view.PlayPressed += OnPlayPressed;
            view.GalleryPressed += OnGalleryPressed;
        }

        private void OnPlayPressed() {
            ScreenController.ShowView(_playView);
        }

        private void OnSettingPressed() {
            ScreenController.ShowView(_settingsView);
        }
        
        private void OnGalleryPressed() {
            ScreenController.ShowView(_galleryView);
        }

        private void OnDisable() {
            view.SettingsPressed -= OnSettingPressed;
            view.GalleryPressed -= OnGalleryPressed;
            view.PlayPressed -= OnPlayPressed;
        }
    }
}