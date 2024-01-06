using FH.Utils;
using System.Collections.Generic;
using UnityEngine;
using YandexSDK.Scripts;

namespace FH.UI.Views.Gallery {
    public class GalleryViewController : ViewController<GalleryView> {
        [SerializeField] private bool _toggleScrollingBgTexture = true;
        [SerializeField] private ViewController _viewOnBack;

        public override void HideView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.DisableRendering();

            view.Hide();
        }

        public override void ShowView() {
            if (_toggleScrollingBgTexture)
                ScrollingBgTextureController.Instance?.EnableRendering();

            YandexMetrika.GalleryOpened();
            view.Show();
        }

        public void SetImages(IEnumerable<Sprite> sprites) {
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
