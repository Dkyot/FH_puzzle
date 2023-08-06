using System;
using System.Collections;
using System.Collections.Generic;
using FH.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Settings {
    public sealed class SettingsView : ViewBase {
        public event Action<float> masterValueChanged;
        public event Action<float> musicValueChanged;
        public event Action<float> sfxValueChanged;

        public event Action donePressed {
            add => _doneButton.clicked += value;
            remove => _doneButton.clicked -= value;
        }

        private Button _doneButton;
        
        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _sfxSlider;

        public override void Init() {
            _masterSlider = this.Q<Slider>("MasterSlider");
            _musicSlider = this.Q<Slider>("MusicSlider");
            _sfxSlider = this.Q<Slider>("SFXSlider");

            _doneButton = this.Q<Button>("DoneButton");
        }

        public void Show() {
            style.display = DisplayStyle.Flex;
            
            _masterSlider.RegisterValueChangedCallback(_OnMasterValueChanged);
            _musicSlider.RegisterValueChangedCallback(_OnMusicValueChanged);
            _sfxSlider.RegisterValueChangedCallback(_OnSfxValueChanged); 
        }

        public void Hide() {
            style.display = DisplayStyle.None;
            
            _masterSlider.UnregisterValueChangedCallback(_OnMasterValueChanged);
            _musicSlider.UnregisterValueChangedCallback(_OnMusicValueChanged);
            _sfxSlider.UnregisterValueChangedCallback(_OnSfxValueChanged);
        }

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

        private void _OnMasterValueChanged(ChangeEvent<float> @event) => masterValueChanged?.Invoke(@event.newValue);
        private void _OnMusicValueChanged(ChangeEvent<float> @event) => musicValueChanged?.Invoke(@event.newValue);
        private void _OnSfxValueChanged(ChangeEvent<float> @event) => sfxValueChanged?.Invoke(@event.newValue);

        public new sealed class UxmlFactory : UxmlFactory<SettingsView, UxmlTraits> { }
    }
}