using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class MainMenuController : VisualElement {
    public new class UxmlFactory : UxmlFactory<MainMenuController, UxmlTraits> { }

    public event Action PlayClicked;
    public event Action SettingsClicked;

    private Button _playButton;
    private Button _settingsButton;

    public void Init() {
        var _buttonsContainer = this.Q<VisualElement>("ButtonContainer");

        _playButton = _buttonsContainer.Q<VisualElement>("PlayButton").Query<Button>("Button");
        _settingsButton = _buttonsContainer.Q<VisualElement>("SettingsButton").Query<Button>("Button");

        _playButton.clicked += OnPlayClicked;
        _settingsButton.clicked += OnSettingsClicked;
    }

    private void OnPlayClicked() {
        PlayClicked?.Invoke();
    }

    private void OnSettingsClicked() {
        PlayClicked?.Invoke();
    }
}
