using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static FH.UI.Views.Settings.SettingsViewController;

namespace FH.UI.Views.Settings {
    public sealed class SettingsView : ViewBase {
        //public event Action<float> MasterValueChanged;
        public event Action<float> MusicValueChanged;
        public event Action<float> SfxValueChanged;

        public event Action DonePressed {
            add => _doneButton.clicked += value;
            remove => _doneButton.clicked -= value;
        }

        public event Action<LocalizationOption> LanguageSelected;

        private Button _doneButton;

        private SimpleSlider _masterSlider;
        private SimpleSlider _musicSlider;
        private SimpleSlider _sfxSlider;

        private VisualElement _languageContainer;

        public override void Show() {
            base.Show();
        }

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

        public void SetLanguages(IEnumerable<LocalizationOption> localizations) {
            foreach (var language in localizations) {
                var button = new RoundButton() {
                    Label = language.localizationNameKey,
                    IsLocalizable = true,
                };

                button.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
                button.clicked += () => LanguageSelected?.Invoke(language);
                button.clicked += InvokeButtonPressed;

                _languageContainer.Add(button);
            }
        }

        protected override void OnInit() {
            var volumeSection = this.Q("VolumeSection");
            _masterSlider = volumeSection.Q<SimpleSlider>("MasterSlider");
            _musicSlider = volumeSection.Q<SimpleSlider>("MusicSlider");
            _sfxSlider = volumeSection.Q<SimpleSlider>("SFXSlider");

            var languageSection = this.Q("LanguageSection");
            _languageContainer = languageSection.Q("LanguageContainer");

            _doneButton = this.Q<Button>("DoneButton");

            //_masterSlider.RegisterValueChangedCallback(OnMasterValueChanged);
            _musicSlider.RegisterValueChangedCallback(OnMusicValueChanged);
            _sfxSlider.RegisterValueChangedCallback(OnSfxValueChanged);

            _doneButton.clicked += InvokeButtonPressed;

            _doneButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        //private void OnMasterValueChanged(ChangeEvent<float> @event) => MasterValueChanged?.Invoke(@event.newValue);
        private void OnMusicValueChanged(ChangeEvent<float> @event) => MusicValueChanged?.Invoke(@event.newValue);
        private void OnSfxValueChanged(ChangeEvent<float> @event) => SfxValueChanged?.Invoke(@event.newValue);

        public new sealed class UxmlFactory : UxmlFactory<SettingsView, UxmlTraits> { }
    }
}