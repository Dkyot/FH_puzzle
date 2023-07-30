using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class SimpleButtonBase : Button {
        public const string buttonName = "BaseButton";
        public const string buttonClassName = "button_with_border";

        public const string buttonBackgroundName = "Background";
        public const string buttonBackgroundClassName = "background";

        // public event Action Clicked {
        //     add { baseButton.clicked += value; }
        //     remove { baseButton.clicked -= value; }
        // }

        protected abstract string BaseStyleName { get; }

        // protected Button baseButton;
        protected VisualElement background;

        public new class UxmlTraits : Button.UxmlTraits {
            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as SimpleButtonBase;

                ate.Clear();

                ate.AddToClassList(buttonClassName);
                ate.AddToClassList(ate.BaseStyleName);

                var background = new VisualElement() {
                    name = buttonBackgroundName
                };

                background.AddToClassList(buttonBackgroundClassName);
                ate.Add(background);
                ate.background = background;
            }

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription {
                get { yield break; }
            }

            public override IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription => base.uxmlAttributesDescription;
        }
    }
}