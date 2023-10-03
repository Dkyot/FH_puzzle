using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Settings {
    public sealed class SettingsView : ViewBase {
        public event Action<float> MasterValueChanged;
        public event Action<float> MusicValueChanged;
        public event Action<float> SfxValueChanged;

        public event Action DonePressed {
            add => _doneButton.clicked += value;
            remove => _doneButton.clicked -= value;
        }

        public event Action LanguageLeftSwitched {
            add => _leftLanguageSwitchButton.clicked += value;
            remove => _leftLanguageSwitchButton.clicked -= value;
        }

        public event Action LanguageRightSwitched {
            add => _rightLanguageSwitchButton.clicked += value;
            remove => _rightLanguageSwitchButton.clicked -= value;
        }

        private Button _doneButton;

        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _sfxSlider;

        private LocalizedLabel _languageLabel;
        private Button _leftLanguageSwitchButton;
        private Button _rightLanguageSwitchButton;

        public void SetMasterValue(float value, bool notifyListeners = false) {
            if (notifyListeners)
                _masterSlider.SetValueWithoutNotify(value);
            else
                _masterSlider.value = value;
        }

        public void SetMusicValue(float value, bool notifyListeners = false) {
            if (notifyListeners)
                _musicSlider.SetValueWithoutNotify(value);
            else
                _musicSlider.value = value;
        }

        public void SetSfxValue(float value, bool notifyListeners = false) {
            if (notifyListeners)
                _sfxSlider.SetValueWithoutNotify(value);
            else
                _sfxSlider.value = value;
        }

        public void SetLanguageNameKey(string key) {
            _languageLabel.Label = key;
        }

        protected override void OnInit() {
            var volumeSection = this.Q("VolumeSection");
            _masterSlider = volumeSection.Q<Slider>("MasterSlider");
            _musicSlider = volumeSection.Q<Slider>("MusicSlider");
            _sfxSlider = volumeSection.Q<Slider>("SFXSlider");

            var languageSection = this.Q("LanguageSection");
            _leftLanguageSwitchButton = languageSection.Q<Button>("LeftLanguageButton");
            _rightLanguageSwitchButton = languageSection.Q<Button>("RightLanguageButton");
            _languageLabel = languageSection.Q<LocalizedLabel>("LanguageLabel");

            _doneButton = this.Q<Button>("DoneButton");

            _masterSlider.RegisterValueChangedCallback(OnMasterValueChanged);
            _musicSlider.RegisterValueChangedCallback(OnMusicValueChanged);
            _sfxSlider.RegisterValueChangedCallback(OnSfxValueChanged);

            _doneButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _leftLanguageSwitchButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _rightLanguageSwitchButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        private void OnMasterValueChanged(ChangeEvent<float> @event) => MasterValueChanged?.Invoke(@event.newValue);
        private void OnMusicValueChanged(ChangeEvent<float> @event) => MusicValueChanged?.Invoke(@event.newValue);
        private void OnSfxValueChanged(ChangeEvent<float> @event) => SfxValueChanged?.Invoke(@event.newValue);

        public new sealed class UxmlFactory : UxmlFactory<SettingsView, UxmlTraits> { }
    }
}