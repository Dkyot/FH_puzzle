using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.MainMenu {
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuController : MonoBehaviour {
        private UIDocument _document;
        private IMainMenu _mainMenu;

        private void Awake() {
            _document = GetComponent<UIDocument>();
            _mainMenu = _document.rootVisualElement.Q<MainMenuContainer>();
            _mainMenu.Init();
        }

        private void Start() {
            _mainMenu.Activate();
        }
    }
}