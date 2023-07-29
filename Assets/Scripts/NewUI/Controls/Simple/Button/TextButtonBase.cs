using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class TextButtonBase : SimpleButtonBase {
        public const string buttonLabelName = "Text";
        public const string buttonTextClassName = "button_text";

        public string Label {
            get => _label;
            set {
                _label = value;
                label.text = value;
            }
        }

        protected Label label;

        private string _label;

        public new class UxmlTraits : SimpleButtonBase.UxmlTraits {
            private UxmlStringAttributeDescription _labelValue = new() { name = "Label", defaultValue = "Text" };

            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as TextButtonBase;

                var text = _labelValue.GetValueFromBag(bag, cc);


                var label = new Label(text) {
                    name = buttonLabelName
                };

                ate.label = label;
                label.AddToClassList(buttonTextClassName);
                ate.background.Add(label);
            }
        }
    }
}