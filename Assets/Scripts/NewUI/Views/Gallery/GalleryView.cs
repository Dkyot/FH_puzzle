using FH.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Gallery {
    public sealed class GalleryView : ViewBase {
        public event Action BackPressed {
            add => _backButton.clicked += value;
            remove => _backButton.clicked -= value;
        }

        //public event Action PhotoCardHovered;

        private Button _backButton;
        private Label _noPhotoLabel;

        private VisualElement _photoContainer;
        private VisualElement _photoScrollView;

        private VisualElement _imageView;
        private VisualElement _imageContainer;

        public void SetImages(IEnumerable<Sprite> sprites) {
            _photoContainer.Clear();
            foreach (var sprite in sprites) {
                var photoCard = new PhotoCard() { usageHints = UsageHints.DynamicTransform };
                photoCard.SetImage(sprite);
                photoCard.RegisterCallback<ClickEvent>((_) => OnImageClicled(sprite));
                photoCard.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
                _photoContainer.Add(photoCard);
            }

            if (_photoContainer.hierarchy.childCount == 0) {
                _photoScrollView.style.display = DisplayStyle.None;
                _noPhotoLabel.style.display = DisplayStyle.Flex;
            }
            else {
                _photoScrollView.style.display = DisplayStyle.Flex;
                _noPhotoLabel.style.display = DisplayStyle.None;
            }
        }

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
            _noPhotoLabel = this.Q<Label>("NoPhotoLabel");

            _photoScrollView = this.Q<VisualElement>("PhotoScrollView");
            _photoContainer = this.Q<VisualElement>("PhotoContainer");

            _imageView = this.Q<VisualElement>("ImageView");
            _imageContainer = this.Q<VisualElement>("ImageContainer");

            _imageView.style.display = DisplayStyle.None;
            _imageView.RegisterCallback<ClickEvent>((_) => _imageView.style.display = DisplayStyle.None);

            _backButton.clicked += InvokeButtonPressed;
            _backButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        //private void OnPhotoHovered(MouseEnterEvent _) {
        //PhotoCardHovered?.Invoke();
        //}

        private void OnImageClicled(Sprite sprite) {
            InvokeButtonPressed();
            _imageView.style.display = DisplayStyle.Flex;
            _imageContainer.style.backgroundImage = new StyleBackground(sprite);
        }

        public new sealed class UxmlFactory : UxmlFactory<GalleryView, UxmlTraits> { }
    }
}
