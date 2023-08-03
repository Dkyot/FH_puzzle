using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class RoundButton : TextButtonBase {
        public const string roundButtonClassName = "round_button";

        protected override string BaseStyleName => roundButtonClassName;

        public new sealed class UxmlFactory : UxmlFactory<RoundButton, UxmlTraits> { }
    }
}