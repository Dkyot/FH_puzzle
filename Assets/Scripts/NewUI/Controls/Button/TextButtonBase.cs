using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class TextButtonBase : SimpleButtonBase {
        public const string buttonLabelName = "Text";
        public const string buttonTextClassName = "button_text";

        public string Label {
            get => label.Label;
            set => label.Label = value;
        }

        public bool IsLocalizable {
            get => label.IsLocalizable;
            set => label.IsLocalizable = value; 
        }

        protected LocalizedLabel label;

        public TextButtonBase() {
            label = new LocalizedLabel() {
                name = buttonLabelName
            };

            label.AddToClassList(buttonTextClassName);
            background.Add(label);
        }

        public new class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription _label = new() { name = "Label", defaultValue = "" };
            private UxmlBoolAttributeDescription _isLocalizable = new() { name = "IsLocalizable", defaultValue = true };

            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as TextButtonBase;
                var text = _label.GetValueFromBag(bag, cc);
                ate.Label = text;

                ate.IsLocalizable = _isLocalizable.GetValueFromBag(bag, cc);
            }
        }
    }
}