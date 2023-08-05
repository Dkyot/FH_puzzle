using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.MainMenu {
    public class MainMenuViewController : ViewController {
        private MainMenuView _mainMenu;

        public override void ShowView() {
            _mainMenu.Activate();
        }

        public override void HideView() {
            _mainMenu.Disable();
        }

        protected override void OnScreenControllerSet() {
            _mainMenu = ScreenController.Document.rootVisualElement.Q<MainMenuView>();
            _mainMenu.Init(); 
        }
    }
}