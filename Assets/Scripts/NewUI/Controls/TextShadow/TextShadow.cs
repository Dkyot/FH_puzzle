using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TextShadow : VisualElement, INotifyValueChanged<string> {
    public const string shadowStyleName = "shadow_label_shadow";
    public const string textStyleName = "shadow_label_text";

    public string value {
        get => _text;
        set {
            if (EqualityComparer<string>.Default.Equals(_text, value)) 
                return;

            if (this.panel != null) {
                using ChangeEvent<string> changeEvent = ChangeEvent<string>.GetPooled(this._text, value);
                changeEvent.target = this;
                SetValueWithoutNotify(value);
                SendEvent(changeEvent);
            }
            else{
                SetValueWithoutNotify(value);
            }
        }
    }

    private string _text;
    private Label _labelShadow;
    private Label _label;

    public TextShadow() { }

    public void SetValueWithoutNotify(string newValue) {
        _text = newValue;
    }

    public new class UxmlFactory : UxmlFactory<TextShadow, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits {
        UxmlStringAttributeDescription _text = new UxmlStringAttributeDescription() { name = "value", defaultValue = "Text" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
            base.Init(ve, bag, cc);
            var ate = ve as TextShadow;

            var text = _text.GetValueFromBag(bag, cc);
            ate._text = text;

            ate.Clear();

            var shadowLabel = new Label(text) {
                name = "LabelShadow"
            };
            shadowLabel.AddToClassList(shadowStyleName);
            shadowLabel.AddToClassList(textStyleName);
            ate._labelShadow = shadowLabel;

            var label = new Label(text) {
                name = "Label"
            };
            label.AddToClassList(textStyleName);
            ate._label = label;

            shadowLabel.Add(label);
            ate.Add(shadowLabel);

            ate.RegisterValueChangedCallback(ate.UpdateText);
        }
    }

    private void UpdateText(ChangeEvent<string> e) {
        _label.text = e.newValue;
        _labelShadow.text = e.newValue;
    }
}
