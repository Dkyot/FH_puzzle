using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace FH.UI {
    public class LocalizedLabel : Label {
        public string Label {
            get => _label;
            set {
                if (_label ==  value) return;   
                _label = value;
                UpdateLable();
            }
        }

        public bool IsLocalizable {
            get => _isLocalizable;
            set {
                _isLocalizable = value;
            }
        }

        private bool _isLocalizable = true;
        private string _label;

        private LocalizedString _localizedString;

        public LocalizedLabel() {
        }

        private void UpdateLable() {
            if (!_isLocalizable) {
                text = _label;
                return;
            }

            if (Application.isPlaying) {
                LoadString();
                return;
            }

            text = $"# {_label}";
        }

        private void LoadString() {
            if (_localizedString != null) {
                _localizedString.StringChanged -= OnStringChanged;
            }

            if (_label is null || _label.Length == 0)
                return;

            _localizedString = new LocalizedString("UI", _label);
            _localizedString.StringChanged += OnStringChanged;
        }

        private void OnStringChanged(string newString) {
            text = newString;
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription _label = new() { name = "Label", defaultValue = "" };
            private UxmlBoolAttributeDescription _isLocalizable = new() { name = "IsLocalizable", defaultValue = true };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var label = ve as LocalizedLabel;
                var isLocalizable = _isLocalizable.GetValueFromBag(bag, cc);
                label.IsLocalizable = isLocalizable;

                var key = _label.GetValueFromBag(bag, cc);
                label.Label = key;
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<LocalizedLabel, UxmlTraits> { }
    }
}
