using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace FH.UI {
    public class LocalizedLabel : Label {
        public string Label {
            get => _label;
            set {
                _label = value;
                UpdateLable();
            }
        }

        public bool IsLocalizable {
            get => _isLocalizable;
            set {
                _isLocalizable = value;
                UpdateIsLocalizable();
            }
        }

        private bool _isLocalizable = true;
        private string _label;

        private AsyncOperationHandle<string>? _handle;

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

        private void UpdateIsLocalizable() {
            if (_isLocalizable) {
                LocalizationSettings.Instance.OnSelectedLocaleChanged += OnLocaleChanged;
                UpdateLable();
            }
            else {
                LocalizationSettings.Instance.OnSelectedLocaleChanged -= OnLocaleChanged;
                UpdateLable();
            }
        }

        private void OnLocaleChanged(Locale locale) {
            LoadString();
        }

        private void LoadString() {
            if (_label is null || _label.Length == 0)
                return;

            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                "UI",
                _label
            );

            if (_handle.HasValue) {
                _handle.Value.Completed -= OnStringLoadCompleted;
            }

            _handle = operation;

            operation.Completed += OnStringLoadCompleted;
        }

        private void OnStringLoadCompleted(AsyncOperationHandle<string> operationHandle) {
            text = operationHandle.Result;
            _handle = null;
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
