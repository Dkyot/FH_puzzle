using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class Rang : VisualElement {
        public const string rangClass = "rang";
        public const string rangTransitionClass = "rang-transition";
        public const string rangAnimationClass = "rang-animation";

        public const string rangSsClass = "rang-ss";
        public const string rangSClass = "rang-s";
        public const string rangAClass = "rang-a";
        public const string rangBClass = "rang-b";
        public const string rangCClass = "rang-c";
        public const string rangDClass = "rang-d";
        
        public RangTypes RangType {
            get => _rangType;
            set {
                _rangType = value;
                _label.RemoveFromClassList(_currentStyle);
                var (text, rangStyle) = RangToTextAndStyle(value);
                _currentStyle = rangStyle;
                _label.AddToClassList(rangStyle);
                _label.text = text;
            }
        }

        private RangTypes _rangType;
        private string _currentStyle;
        
        private readonly Label _label;
        
        public Rang() {
            _label = new Label();
            _label.ClearClassList();
            _label.AddToClassList(rangClass);
            Add(_label);

            var (text, rangStyle) = RangToTextAndStyle(_rangType);
            _label.text = text;
            _currentStyle = rangStyle;
            _label.AddToClassList(rangStyle);
        }
        
        public void StartAnimation() {
            RemoveFromClassList(rangTransitionClass);
            AddToClassList(rangAnimationClass);
        }

        public void ResetAnimation() {
            RemoveFromClassList(rangAnimationClass);
            AddToClassList(rangTransitionClass);
        }
        
        private static (string text, string style) RangToTextAndStyle(RangTypes rangType) => (rangType) switch {
            RangTypes.SS => ("SS", rangSsClass),
            RangTypes.S => ("S", rangSClass),
            RangTypes.A => ("A", rangAClass),
            RangTypes.B => ("B", rangBClass),
            RangTypes.C => ("C", rangCClass),
            RangTypes.D => ("D", rangDClass),
            _ => throw new ArgumentOutOfRangeException(nameof(rangType), rangType, null)
        };

        public enum RangTypes {
            SS,
            S,
            A,
            B,
            C,
            D
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlEnumAttributeDescription<RangTypes> _type = new() {name = "Rang", defaultValue = RangTypes.SS};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var rang = ve as Rang;
                var type = _type.GetValueFromBag(bag, cc);
                rang.RangType = type;
            }
        }
        
        public new sealed class UxmlFactory : UxmlFactory<Rang, UxmlTraits> { }
    }
}