using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Gallery {
    public class GalleryView : ViewBase {
        private Button _backButton;
        private VisualElement _photoContainer;

        protected override void OnInit() {
            _backButton = this.Q<Button>("BackButton");
            _photoContainer = this.Q<VisualElement>("PhotoContainer");
        }

        public new sealed class UxmlFactory : UxmlFactory<GalleryView, UxmlTraits> { }
    }
}
