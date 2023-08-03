using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class CircleIconButton : IconButtonBase {
        public const string circeButtonClassName = "circle_button";

        protected override string BaseStyleName => circeButtonClassName;

        public new sealed class UxmlFactory : UxmlFactory<CircleIconButton, UxmlTraits> { }
    }
}