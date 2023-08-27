using System;
using UnityEngine;
using UnityEngine.UIElements;
using static FH.UI.Rang;

namespace FH.UI {
    public class LevelOption : VisualElement {
        private const string _containerClass = "level-option-container";
        private const string _levelNumberClass = "level-option-number";
        private const string _levelOptionRangClass = "level-option-rang";
        private const string _enabledClass = "level-option-enabled";
        private const string _completedClass = "level-option-completed";

        public event Action<LevelOption> Pressed;

        public bool IsEnabled {
            get => _isEnabled;
            set {
                _isEnabled = value;
                SetEnable(value);
            }
        }

        public bool IsCompleted {
            get => _isCompleted;
            set {
                _isCompleted = value;
                SetCompleted(value);
            }
        }

        public string LevelNumber {
            get => _levelNumber;
            set {
                _levelNumber = value;
                SetNumber(value);
            }
        }

        public RangTypes Rang {
            get => _rangType;
            set {
                _rangType = value;
                SetRang(value);
            }
        }

        private readonly Rang _levelRang;
        private readonly Label _levelNumberLabel;

        private bool _isEnabled;
        private bool _isCompleted;
        private string _levelNumber;
        private RangTypes _rangType;

        public LevelOption() {
            AddToClassList(_containerClass);

            _levelNumberLabel = new Label();
            _levelNumberLabel.AddToClassList(_levelNumberClass);
            Add(_levelNumberLabel);

            _levelRang = new Rang();
            _levelRang.AddToClassList(_levelOptionRangClass);
            Add(_levelRang);

            RegisterCallback<ClickEvent>(OnPressed);
        }

        private void SetRang(RangTypes rang) {
            _levelRang.RangType = rang;
        }

        private void SetNumber(string number) {
            _levelNumberLabel.text = number;
        }

        private void SetEnable(bool value) {
            if (value) {
                AddToClassList(_enabledClass);
                return;
            }
            RemoveFromClassList(_enabledClass);
        }

        private void SetCompleted(bool value) {
            if (value) {
                AddToClassList(_completedClass);
                return;
            }
            RemoveFromClassList (_completedClass);
        }

        private void OnPressed(ClickEvent @event) {
            // if (@event.propagationPhase != PropagationPhase.AtTarget)
                // return;

            Pressed?.Invoke(this);
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlEnumAttributeDescription<RangTypes> _rang = new() { name = "Rang", defaultValue = RangTypes.SS };
            private UxmlStringAttributeDescription _levelNumber = new() { name = "Number", defaultValue = null };
            private UxmlBoolAttributeDescription _isEnabled = new() { name = "IsEnabled", defaultValue = true };
            private UxmlBoolAttributeDescription _isCompleted = new () { name = "IsCompleted", defaultValue= false };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var level = ve as LevelOption;
                var type = _rang.GetValueFromBag(bag, cc);
                level.Rang = type;

                var isEnabled = _isEnabled.GetValueFromBag(bag, cc);
                level.IsEnabled = isEnabled;

                var levelNumber = _levelNumber.GetValueFromBag(bag, cc);
                level.LevelNumber = levelNumber;

                var isComleted = _isCompleted.GetValueFromBag(bag, cc);
                level.IsCompleted = isComleted;
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelOption, UxmlTraits> { }
    }
}
