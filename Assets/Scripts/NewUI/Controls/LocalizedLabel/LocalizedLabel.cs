using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace FH.UI {
    public class LocalizedLabel : Label {
        public string Key {
            get => _key;
            set {
                _key = value;
                UpdateKey();
            }
        }

        private string _key;

        public LocalizedLabel() {
            if (Application.isPlaying) {
                LocalizationSettings.Instance.OnSelectedLocaleChanged += OnLocaleChanged;
            }
        }

        private void UpdateKey() {
            if (Application.isPlaying) {
                LoadString();
                return;
            }

            text = $"# {_key}";
        }

        private void OnLocaleChanged(Locale locale) {
            LoadString();
        }

        private void LoadString() {
            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                "UI", 
                _key
            );

            operation.Completed += OnStringLoadCompleted;
        }

        private void OnStringLoadCompleted(AsyncOperationHandle<string> operationHandle) {
            text = operationHandle.Result;
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription _key = new() { name = "Key", defaultValue = "" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var label = ve as LocalizedLabel;
                var key = _key.GetValueFromBag(bag, cc);
                label.Key = key;
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<LocalizedLabel, UxmlTraits> { }
    }
}
