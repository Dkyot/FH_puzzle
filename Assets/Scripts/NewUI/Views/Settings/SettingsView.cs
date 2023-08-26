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

        private Button _doneButton;
        
        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _sfxSlider;

        public void SetMasterValue(float value, bool notifyListeners = false) {
            if (notifyListeners) _masterSlider.SetValueWithoutNotify(value);
            else _masterSlider.value = value;
        }

        public void SetMusicValue(float value, bool notifyListeners = false) {
            if (notifyListeners) _musicSlider.SetValueWithoutNotify(value);
            else _musicSlider.value = value; 
        }

        public void SetSfxValue(float value, bool notifyListeners = false) {
            if (notifyListeners) _sfxSlider.SetValueWithoutNotify(value);
            else _sfxSlider.value = value; 
        }

        protected override void OnInit() {
            _masterSlider = this.Q<Slider>("MasterSlider");
            _musicSlider = this.Q<Slider>("MusicSlider");
            _sfxSlider = this.Q<Slider>("SFXSlider");

            _doneButton = this.Q<Button>("DoneButton");

            _masterSlider.RegisterValueChangedCallback(OnMasterValueChanged);
            _musicSlider.RegisterValueChangedCallback(OnMusicValueChanged);
            _sfxSlider.RegisterValueChangedCallback(OnSfxValueChanged); 
        }

        private void OnMasterValueChanged(ChangeEvent<float> @event) => MasterValueChanged?.Invoke(@event.newValue);
        private void OnMusicValueChanged(ChangeEvent<float> @event) => MusicValueChanged?.Invoke(@event.newValue);
        private void OnSfxValueChanged(ChangeEvent<float> @event) => SfxValueChanged?.Invoke(@event.newValue);

        public new sealed class UxmlFactory : UxmlFactory<SettingsView, UxmlTraits> { }
    }
}