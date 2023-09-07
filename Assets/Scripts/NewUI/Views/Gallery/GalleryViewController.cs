using FH.SO;
using FH.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.UI.Views.Gallery {
    public sealed class GalleryViewController : ViewController<GalleryView> {
        [SerializeField] private ViewController _viewOnBack;

        public override void HideView() {
            view.Hide();
            ScrollingBgTextureController.Instance?.DisableRendering();
        }

        public override void ShowView() {
            view.Show();
            ScrollingBgTextureController.Instance?.EnableRendering();
        }

        public void SetInages(IEnumerable<Sprite> sprites) {
            view.SetImages(sprites);
        }

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.BackPressed += OnBack;
        }

        private void OnBack() {
            ScreenController.ShowView(_viewOnBack);
        }

        private void OnDisable() {
            view.BackPressed -= OnBack;
        }
    }
}
