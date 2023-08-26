using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Gallery {
    public class GalleryView : ViewBase {
        public event Action BackPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        private Button _backButton;
        private Label _noPhotoLabel;
        private VisualElement _photoContainer;
        private VisualElement _photoScrollView;

        public void SetPhotos() { }

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
            _noPhotoLabel = this.Q<Label>("NoPhotoLabel");
            _photoScrollView = this.Q<VisualElement>("PhotoScrollView");
            _photoContainer = this.Q<VisualElement>("PhotoContainer");
        }

        public new sealed class UxmlFactory : UxmlFactory<GalleryView, UxmlTraits> { }
    }
}