using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuComponent : MonoBehaviour {
    private MainMenuController _menuController;

    private void Awake() {
        var document = GetComponent<UIDocument>();
        _menuController = document.rootVisualElement.Q<MainMenuController>();
        
        _menuController.Init();
    }
}
