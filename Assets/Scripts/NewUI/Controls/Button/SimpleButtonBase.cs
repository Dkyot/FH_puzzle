using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class SimpleButtonBase : Button {
        public const string buttonName = "BaseButton";
        public const string buttonClassName = "base-button";

        public const string buttonBackgroundName = "Background";
        public const string buttonBackgroundClassName = "background";

        // public event Action Clicked {
        //     add { baseButton.clicked += value; }
        //     remove { baseButton.clicked -= value; }
        // }

        protected abstract string BaseStyleName { get; }

        protected VisualElement background;

        public SimpleButtonBase() : base() {
            usageHints = UsageHints.MaskContainer | UsageHints.DynamicColor;
            AddToClassList(buttonClassName);
            AddToClassList(BaseStyleName);

            background = new VisualElement() {
                usageHints = UsageHints.DynamicColor | UsageHints.DynamicTransform,
                name = buttonBackgroundName,
            };

            background.AddToClassList(buttonBackgroundClassName);
            Add(background);
        }
    }
}