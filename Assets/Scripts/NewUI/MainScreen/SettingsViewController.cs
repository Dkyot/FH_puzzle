using FH.Utils;
using UnityEngine;

namespace FH.UI.MainMenu {
    public class SettingsViewController : ViewController {
        [SerializeField] private ScrollingBgTextureController _bgTextureController;
        public override void ShowView() {
            _bgTextureController.EnableRendering();
        }

        public override void HideView() {
            _bgTextureController.DisableRendering();
        }

        protected override void OnScreenControllerSet() {
        }
    }
}